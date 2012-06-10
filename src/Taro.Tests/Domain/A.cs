using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Tests.Domain.Events;

namespace Taro.Tests.Domain
{
    public class A
    {
        public virtual int Id { get; set; }

        public virtual int Value { get; set; }

        public virtual void AddValue(int value)
        {
            Value += value;
            DomainEvent.Apply(new AValueAdded { ValueToAdd = value });
        }
    }
}
