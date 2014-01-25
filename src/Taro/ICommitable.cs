using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Dispatching;

namespace Taro
{
    public interface ICommitable : IDisposable
    {
        event EventHandler Comitted;

        void Commit();
    }
}
