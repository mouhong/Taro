using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro
{
    public interface IEvent
    {
        Guid Id { get; }

        DateTimeOffset UtcCreationTime { get; }
    }

    public abstract class Event : IEvent
    {
        public Guid Id { get; set; }

        public DateTimeOffset UtcCreationTime { get; set; }

        public Event()
        {
            Id = Guid.NewGuid();
            UtcCreationTime = DateTimeOffset.UtcNow;
        }
    }
}
