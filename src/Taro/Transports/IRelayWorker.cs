using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro.Transports
{
    // TODO: RelayWorker should be able to run in a separate process or server.
    //       Because there might be multi web apps using one database.
    public interface IRelayWorker
    {
        void Start();

        void Signal();

        Task Stop();
    }
}
