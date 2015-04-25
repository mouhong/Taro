using System;
using System.ComponentModel;

namespace Taro.Configuration
{
    public class AppConfigurator : HideObjectMembers
    {
        public AppRuntime AppRuntime { get; private set; }

        public AppConfigurator(AppRuntime appRuntime)
        {
            AppRuntime = appRuntime;
        }
    }
}
