using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Taro.Persistence;
using Taro.Transports;

namespace Taro.Workers
{
    public class RelayWorker : IRelayWorker
    {
        private Task _task;
        private TaskCompletionSource<int> _stopPromise;
        private ManualResetEventSlim _eventStoreNotEmptyEvent;
        private bool _stopRequested;

        private IDomainDbSessionFactory _dbSessionFactory;
        private IEventTransport _transport;

        private readonly object _startLock = new object();

        private int _signals;
        private readonly object _signalsLock = new object();

        public bool Started { get; private set; }

        private int _batchSize = 100;

        public int BatchSize
        {
            get { return _batchSize; }
        }

        public RelayWorker(IDomainDbSessionFactory dbSessionFactory, IEventTransport transport)
            : this(dbSessionFactory, transport, -1)
        {
        }

        public RelayWorker(IDomainDbSessionFactory dbSessionFactory, IEventTransport transport, int batchSize)
        {
            _dbSessionFactory = dbSessionFactory;
            _transport = transport;

            if (batchSize > 0)
            {
                _batchSize = batchSize;
            }
        }

        public void Start()
        {
            lock (_startLock)
            {
                if (Started)
                {
                    return;
                }

                Started = true;
            }

            _stopPromise = new TaskCompletionSource<int>();
            _eventStoreNotEmptyEvent = new ManualResetEventSlim(false);
            _task = Task.Factory.StartNew(CheckAndPublishEvents)
                                .ContinueWith(it =>
                                {
                                    OnStopping();
                                });

            Signal();
        }

        private void CheckAndPublishEvents()
        {
            while (!_stopRequested)
            {
                _eventStoreNotEmptyEvent.Wait();

                while (true)
                {
                    // TODO: Error handling
                    using (var session = _dbSessionFactory.OpenSession())
                    {
                        var result = session.FetchEvents(BatchSize);

                        foreach (var storedEvent in result.Events)
                        {
                            var theEvent = session.UnwrapEvent(storedEvent);
                            _transport.Send(theEvent);
                            session.DeleteEvent(storedEvent);
                            session.Commit();
                        }

                        if (!result.HasMore)
                        {
                            break;
                        }
                    }
                }

                // When the worker is signaled, it will fetch events from event store and publish them.
                // There might be new signals arrives after events are fetched,
                // in this case, we should not reset the _eventStoreNotEmptyEvent because the event store becomes non empty again.
                // 
                // So we maintain a _signals variable to avoid signal missing, whose value can be only 0, 1 or 2:
                //  If _signals == 0 
                //    -> There's no new signal, in this case, we will not run to here because we can only get here on signaled.
                //  If _signals == 1 
                //    -> There's only one signal, the one who wake up the worker.
                //  If _signals == 2
                //    -> One or more singals arrive after the one wake up the worker. It means the event store becomes non empty again.
                //
                // If new signals arrive after the worker is woke up but before the events are fetched, we will waste one event store check.
                // Because in this case, we still be able to fetch all new events from event store.
                // But we will only waste one event store check, because _signals will be at most 2.

                Interlocked.Decrement(ref _signals);

                lock (_signalsLock)
                {
                    Debug.WriteLine("[" + DateTime.Now + "] signals: " + _signals);

                    // After the decrement of _signals, if _signals == 0, it means that there's no new signal arriving after the worker is woke up.
                    if (_signals == 0)
                    {
                        _eventStoreNotEmptyEvent.Reset();
                    }
                }
            }
        }

        public void Signal()
        {
            lock (_signalsLock)
            {
                if (_signals < 2)
                {
                    _signals++;
                }

                Debug.WriteLine("[" + DateTime.Now + "] signals: " + _signals);
            }

            _eventStoreNotEmptyEvent.Set();
        }

        public Task Stop()
        {
            _stopRequested = true;
            _eventStoreNotEmptyEvent.Set();

            return _stopPromise.Task;
        }

        private void OnStopping()
        {
            Debug.WriteLine("[" + DateTime.Now + "] RelayWorker stopping.");

            _stopRequested = false;
            _eventStoreNotEmptyEvent.Dispose();
            _eventStoreNotEmptyEvent = null;
            _task = null;
            _stopPromise.SetResult(0);

            Started = false;
        }
    }
}
