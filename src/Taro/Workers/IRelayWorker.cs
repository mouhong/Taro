using System.Threading.Tasks;

namespace Taro.Workers
{
    public interface IRelayWorker
    {
        void Start();

        void Signal();

        Task Stop();
    }
}
