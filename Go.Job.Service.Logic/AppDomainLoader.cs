using System;

namespace Go.Job.Service.Logic
{
    /// <summary>
    /// 这是一个核心类,非常核心
    /// </summary>
    public static class AppDomainLoader
    {
        /// <summary>
        /// 加载应用程序，获取作业实例
        /// </summary>
        /// <param name="assemblyPath">作业实例程序集的物理路径</param>
        /// <param name="classType">作业实例的完全限定名</param>
        /// <param name="appDomain"></param>
        /// <returns></returns>
        public static BaseJob.BaseJob Load(string assemblyPath, string classType, out AppDomain appDomain)
        {
            AppDomainSetup setup = new AppDomainSetup();
            if (System.IO.File.Exists($"{assemblyPath}.config"))
            {
                setup.ConfigurationFile = $"{assemblyPath}.config";
            }
            setup.ShadowCopyFiles = "true";
            setup.ApplicationBase = System.IO.Path.GetDirectoryName(assemblyPath);
            string appDomainName = System.IO.Path.GetFileName(assemblyPath);
            if (string.IsNullOrWhiteSpace(appDomainName))
            {
                throw new ArgumentNullException($"应用程序域名称为空,assemblyPath : {assemblyPath}");
            }
            appDomain = AppDomain.CreateDomain(appDomainName, null, setup);
            AppDomain.MonitoringIsEnabled = true;
            BaseJob.BaseJob obj = (BaseJob.BaseJob)appDomain.CreateInstanceFromAndUnwrap(assemblyPath, classType);
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
