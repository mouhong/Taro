namespace Taro.Configuration
{
    public class AppConfigurator : IHideObjectMembers
    {
        public AppRuntime AppRuntime { get; private set; }

        public AppConfigurator(AppRuntime appRuntime)
        {
            AppRuntime = appRuntime;
        }
    }
}
