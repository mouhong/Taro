using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;

namespace Taro.Events.Storage.RavenDB.Indexes
{
    public class EventDocumentIndex : AbstractIndexCreationTask<EventDocument>
    {
        public EventDocumentIndex()
        {
            Map = events => from e in events
                            select new
                            {
                                e.Id,
                                e.UtcTimestamp
                            };

            Index(e => e.Id, Raven.Abstractions.Indexing.FieldIndexing.NotAnalyzed);
            Index(e => e.UtcTimestamp, Raven.Abstractions.Indexing.FieldIndexing.NotAnalyzed);
        }
    }
}
