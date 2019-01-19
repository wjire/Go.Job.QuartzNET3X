using System;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Go.Job.Service.Config
{

    internal static class AppSettingsConfig
    {

        internal static string SchedName => AppSettingValue();


        internal static string ApiAddress => AppSettingValue();
            

        private static string AppSettingValue([CallerMemberName] string key = null)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
