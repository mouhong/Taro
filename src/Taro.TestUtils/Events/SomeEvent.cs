using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Taro.TestUtils.Events
{
    [DataContract]
    public class SomeEvent : Event
    {
        [DataMember]
        public string SomeProperty { get; set; }

        public SomeEvent()
        {
        }
    }
}
