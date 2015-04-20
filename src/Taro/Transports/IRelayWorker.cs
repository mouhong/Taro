using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro.Transports
{
    public interface IRelayWorker
    {
        void Start();

        void Signal();

        Task Stop();
    }
}
