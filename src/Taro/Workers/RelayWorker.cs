using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Taro.Persistence;
using Taro.Transports;

namespace Taro.Workers
{
    public sealed class RelayWorker : IRelayWorker
    {
        private Task _task;
        private ManualResetEventSlim _eventStoreNotEmptyEvent;
        private bool _stopRequested;

        private Func<IDomainDbSession> _openDbSession;
        private IEventTransport _transport;

        private readonly object _startStopLock = new object();

        private int _signals;
        private readonly object _signalsLock = new object();

        public bool Started { get; private set; }

        private int _batchSize = 100;

        public int BatchSize
        {
            get { return _batchSize; }
        }

        public RelayWorker(Func<IDomainDbSession> openDbSession, IEventTransport transport)
            : this(openDbSession, transport, -1)
        {
        }

        public RelayWorker(Func<IDomainDbSession> openDbSession, IEventTransport transport, int batchSize)
        {
            Require.NotNull(openDbSession, "openDbSession");
            Require.NotNull(transport, "transport");

            _openDbSession = openDbSession;
            _transport = transport;

            if (batchSize > 0)
            {
                _batchSize = batchSize;
            }
        }

        public void Start()
        {
            lock (_startStopLock)
            {
                if (Started)
                {
                    return;
                }

                _eventStoreNotEmptyEvent = new ManualResetEventSlim(false);
                _task = Task.Factory.StartNew(CheckAndPublishEvents, TaskCreationOptions.LongRunning);

                Started = true;

                Signal();
            }
        }

        private void CheckAndPublishEvents()
        {
            while (Volatile.Read(ref _stopRequested) == false)
            {
                _eventStoreNotEmptyEvent.Wait();

                while (true)
                {
                    // TODO: Error handling
                    using (var session = _openDbSession())
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
            var started = Started;
            if (!started)
                throw new InvalidOperationException("Relay worker is not yet started. Start it before sending signals.");

            // It's possible that the relay worker is stopped here by some other thread.
            // In this case, the rest code in this method will fail.
            // But this is rare because Stop is supposed to be called only once when the process is shutdown.
            // So here we ignore this rare case to make signaling simpler.

            // But we still check and throw exception if the relay server is not started at the beginning of this method,
            // in case the programmer forget to start the relay server.

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

        public void Stop(bool waitUntilStopped = true)
        {
            lock (_startStopLock)
            {
                if (!Started)
                {
                    return;
                }

                _stopRequested = true;
                _eventStoreNotEmptyEvent.Set();

                if (waitUntilStopped)
                {
                    _task.Wait();
                }

                Cleanup();
            }
        }

        private void Cleanup()
        {
            Debug.WriteLine("[" + DateTime.Now + "] RelayWorker stopping.");

            _stopRequested = false;
            _eventStoreNotEmptyEvent.Dispose();
            _eventStoreNotEmptyEvent = null;
            _task.Dispose();
            _task = null;

            Started = false;
        }
    }
}
