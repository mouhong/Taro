using Raven.Client.Indexes;
using System.Linq;

namespace Taro.RavenDB.Persistence.Indexes
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
