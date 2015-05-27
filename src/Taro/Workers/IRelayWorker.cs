using System.Threading.Tasks;

namespace Taro.Workers
{
    public interface IRelayWorker
    {
        void Start();

        void Signal();

        void Stop(bool waitUntilStopped = true);
    }
}
