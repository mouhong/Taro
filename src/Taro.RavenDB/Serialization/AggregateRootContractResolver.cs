using Raven.Imports.Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro.RavenDB.Serialization
{
    public class AggregateRootContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, Raven.Imports.Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (member is System.Reflection.PropertyInfo
                && typeof(IEventSource).IsAssignableFrom(member.ReflectedType) 
                && member.Name == "Events" 
                && member.DeclaringType == typeof(EventStream))
            {
                property.ShouldSerialize = _ => false;
            }

            return property;
        }
    }
}
