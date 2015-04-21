using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro.Persistence.RavenDB.Indexes
{
    public class StoredEventIndex : AbstractIndexCreationTask<StoredEvent>
    {
        public StoredEventIndex()
        {
            Map = events => from evnt in events
                            select new
                            {
                                evnt.Body,
                                evnt.UtcCreationTime
                            };
        }
    }
}
