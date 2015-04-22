using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Taro.Notification
{
    public class HttpRemoteNewEventNotifier : INewEventNotifier
    {
        private string _serverUrl;
        private string _notificationApiUrl;

        public HttpRemoteNewEventNotifier(string serverUrl)
        {
            Require.NotNullOrWhitespace(serverUrl, "serverUrl");

            _serverUrl = serverUrl.TrimEnd('/');
            _notificationApiUrl = _serverUrl + "/notification";
        }

        public void Notify()
        {
            var request = WebRequest.Create(_notificationApiUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.GetResponseAsync().ContinueWith(t =>
            {
                // TODO: Logging
            });
        }
    }
}
