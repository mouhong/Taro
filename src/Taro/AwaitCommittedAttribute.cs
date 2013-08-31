using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AwaitCommittedAttribute : Attribute
    {
    }
}
