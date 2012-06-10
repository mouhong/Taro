using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Linq;

namespace BookStore.Domain.Services
{
    public class WebsiteInfoService
    {
        private ISession _session;

        public WebsiteInfoService(ISession session)
        {
            _session = session;
        }

        public string GetValue(string key)
        {
            var item = _session.Get<WebsiteInfo>(key);
            return item == null ? null : item.Value;
        }

        public string GetValue(string key, string defaultValue)
        {
            var value = GetValue(key);
            if (value == null)
            {
                value = defaultValue;
            }

            return value;
        }

        public void SetValue(string key, string value)
        {
            var item = _session.Get<WebsiteInfo>(key);
            if (item == null)
            {
                item = new WebsiteInfo(key, value);
                _session.Save(item);
            }
            item.Value = value;
        }
    }
}
