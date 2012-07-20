using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore
{
    public class WebsiteInfo
    {
        public virtual string Key { get; protected set; }

        public virtual string Value { get; set; }

        protected WebsiteInfo()
        {
        }

        public WebsiteInfo(string key)
            : this(key, null)
        {
        }

        public WebsiteInfo(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
