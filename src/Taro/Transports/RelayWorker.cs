using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Taro.Persistence;

namespace Taro.Transports
{
    public class RelayWorker
    {
        private Task _task;
        private TaskCompletionSource<int> _stopPromise;
        private ManualResetEventSlim _checkingEvent;
        private bool _stopRequested;

        private ILocalEventStore _localEventStore;
        private IEventTransport _dispatcher;

        public RelayWorker(ILocalEventStore localEventStore, IEventTransport eventDispatcher)
        {
            _localEventStore = localEventStore;
            _dispatcher = eventDispatcher;
        }

        public void Start()
        {
            _stopPromise = new TaskCompletionSource<int>();
            _checkingEvent = new ManualResetEventSlim(true);
            _task = Task.Factory.StartNew(CheckAndPublishEvents)
                                .ContinueWith(it =>
                                {
                                    OnStopping();
                                });
        }

        private void CheckAndPublishEvents()
        {
            while (!_stopRequested)
            {
                foreach (var storedEvent in _localEventStore.Enumerate())
                {
                    // TODO: Exception handling
                    _dispatcher.Send(storedEvent.Unwrap());
                    _localEventStore.Delete(storedEvent);
                }

                _checkingEvent.Wait();
            }
        }

        public void Signal()
        {
            _checkingEvent.Set();
        }

        public Task Stop()
        {
            _stopRequested = true;
            return _stopPromise.Task;
        }

        private void OnStopping()
        {
            _stopRequested = false;
            _checkingEvent.Dispose();
            _checkingEvent = null;
            _task = null;
            _stopPromise.SetResult(0);
        }
    }
}
