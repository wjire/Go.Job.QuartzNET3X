using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web.Caching;

namespace Go.Job.Web.Helper
{
    /// <summary>
    /// api地址帮助类
    /// </summary>
    public static class ApiAddressHelper
    {

        /// <summary>
        /// 获取可用的 api 地址
        /// </summary>
        /// <param name="schedName">调度组名称</param>
        /// <returns></returns>
        public static string GetApiAddress(string schedName)
        {
            string addressStr = GetCache(schedName);
            if (string.IsNullOrWhiteSpace(addressStr))
            {
                return null;
            }

            string[] arr = addressStr.Split(',');
            return arr.Length <= 0 ? null : arr.FirstOrDefault(address => PortInUse(address));
        }


        /// <summary>
        /// 检查地址的端口是否被占用
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private static bool PortInUse(string address)
        {
            int port = GetPort(address);
            return PortInUse(port);
        }


        /// <summary>
        /// 检查端口是否被占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private static bool PortInUse(int port)
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();
            return ipEndPoints.Any(endPoint => endPoint.Port == port);
        }


        /// <summary>
        /// 获取api地址中的端口号
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private static int GetPort(string address)
        {
            string[] portArray = address.Split(':');
            string portStr = portArray[2].Trim(' ');
            if (portStr.EndsWith("/"))
            {
                portStr = portStr.TrimEnd('/');
            }

            if (int.TryParse(portStr, out int res))
            {
                return res;
            }

            throw new InvalidCastException("端口号含有非法字符");
        }



        /// <summary>
        /// 从缓存中获取地址
        /// </summary>
        /// <param name="schedName"></param>
        /// <returns></returns>
        private static string GetCache(string schedName)
        {
            string apiAddress = CacheHelper.GetCache<string>(schedName);
            if (!string.IsNullOrWhiteSpace(apiAddress))
            {
                return apiAddress;
            }

            apiAddress = System.Configuration.ConfigurationManager.AppSettings[schedName];
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Web.Config");
            CacheHelper.SetCache(schedName, apiAddress, new CacheDependency(path));
            return apiAddress;
        }
    }
}