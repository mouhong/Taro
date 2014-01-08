using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Events;

namespace Taro
{
    public interface IUnitOfWork : IDisposable
    {
        event EventHandler Comitted;

        void Commit();
    }
}
