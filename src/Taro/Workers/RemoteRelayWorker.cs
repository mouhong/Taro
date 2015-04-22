using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Taro.Workers
{
    public class RemoteRelayWorker : IRelayWorker
    {
        private string _serverUrl;
        private string _notificationApiUrl;

        public RemoteRelayWorker(string serverUrl)
        {
            Require.NotNullOrWhitespace(serverUrl, "serverUrl");

            _serverUrl = serverUrl.TrimEnd('/');
            _notificationApiUrl = _serverUrl + "/notification";
        }

        public void Start()
        {
        }

        public void Signal()
        {
            var request = WebRequest.Create(_notificationApiUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.GetResponseAsync().ContinueWith(t =>
            {
                // TODO: Logging
            });
        }

        public Task Stop()
        {
            return Task.FromResult<int>(0);
        }
    }
}
