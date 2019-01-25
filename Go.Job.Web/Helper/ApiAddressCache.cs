using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web.Caching;

namespace Go.Job.Web.Helper
{
    /// <summary>
    /// api地址缓存
    /// </summary>
    public static class ApiAddressCache
    {

        /// <summary>
        /// 获取可用的 api 地址
        /// </summary>
        /// <param name="schedName">调度组名称</param>
        /// <returns></returns>
        public static string GetApiAddress(string schedName)
        {
            string apiAddress = CacheHelper.GetCache<string>(schedName);
            if (!string.IsNullOrWhiteSpace(apiAddress) && PortInUse(apiAddress))
            {
                return apiAddress;
            }

            return GetAvailableAddress(schedName);
        }


        /// <summary>
        /// 获取可用的api地址
        /// </summary>
        /// <param name="schedName"></param>
        /// <returns></returns>
        private static string GetAvailableAddress(string schedName)
        {
            string apiAddressStr = System.Configuration.ConfigurationManager.AppSettings[schedName];
            if (string.IsNullOrWhiteSpace(apiAddressStr))
            {
                return null;
            }

            string[] arr = apiAddressStr.Split(',');
            string apiAddress = arr.Length <= 0 ? null : arr.FirstOrDefault(PortInUse);
            if (string.IsNullOrWhiteSpace(apiAddress))
            {
                return apiAddress;
            }

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "Web.Config");
            CacheHelper.SetCache(schedName, apiAddress, new CacheDependency(path));

            return apiAddress;
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
    }
}