﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class HandleAsyncAttribute : Attribute
    {
    }
}
