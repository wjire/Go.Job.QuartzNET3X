using System;
using Go.Job.BaseJob;

namespace Go.Job.Service
{
    /// <summary>
    /// 这是一个核心类,非常核心
    /// </summary>
    public class AppDomainLoader
    {
        /// <summary>
        /// 加载应用程序，获取作业实例
        /// </summary>
        /// <param name="dllPath">作业实例程序集的物理路径,含扩展名</param>
        /// <param name="classPath">作业实例的完全限定名,含命名空间</param>
        /// <param name="appDomain"></param>
        /// <returns></returns>
        public static BaseJob.BaseJob Load(string dllPath, string classPath, out AppDomain appDomain)
        {
            AppDomainSetup setup = new AppDomainSetup();
            if (System.IO.File.Exists($"{dllPath}.config"))
            {
                setup.ConfigurationFile = $"{dllPath}.config";
            }
            setup.ShadowCopyFiles = "true";
            setup.ApplicationBase = System.IO.Path.GetDirectoryName(dllPath);
            string appDomainName = System.IO.Path.GetFileName(dllPath);
            appDomain = AppDomain.CreateDomain(appDomainName, null, setup);
            AppDomain.MonitoringIsEnabled = true;
            BaseJob.BaseJob obj = (BaseJob.BaseJob)appDomain.CreateInstanceFromAndUnwrap(dllPath, classPath);
            return obj;
        }

        /// <summary>
        /// 卸载应用程序域
        /// </summary>
        /// <param name="appDomain"></param>
        public static void UnLoad(AppDomain appDomain)
        {
            AppDomain.Unload(appDomain);
            appDomain = null;
        }
    }
}
