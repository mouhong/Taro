using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro.Persistence.NHibernate
{
    public class StoredEvent : IStoredEvent
    {
        public virtual Guid Id { get; set; }

        public virtual string Body { get; set; }

        public virtual DateTimeOffset UtcCreationTime { get; set; }
    }
}
