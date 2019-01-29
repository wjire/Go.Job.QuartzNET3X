using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using Microsoft.Owin.Hosting;

namespace Go.Job.Service.Api
{
    /// <summary>
    /// jobApi启动类
    /// </summary>
    public sealed class JobApiStartHelper
    {

        /// <summary>
        /// 开启监听
        /// </summary>
        /// <param name="schedulerName">调度器名称</param>
        public static void Start(string schedulerName)
        {
            if (string.IsNullOrWhiteSpace(ApiConfig.ApiAddress))
            {
                throw new ArgumentNullException("监听地址不能为空,请在配置文件<appSettings>节点中设置 key=\"ApiAddress\" 的值");
            }

            if (PortInUse(ApiConfig.ApiAddress))
            {
                throw new ArgumentException($"{ApiConfig.ApiAddress} 该地址已被监听!请更换");
            }

            ApiConfig.SchedulerName = schedulerName;
            using (WebApp.Start(ApiConfig.ApiAddress))
            {
                Console.WriteLine($"调度服务监听已启动! 当前监听地址 : {ApiConfig.ApiAddress}");
                string userCommand = string.Empty;
                while (userCommand != "exit")
                {
                    if (string.IsNullOrEmpty(userCommand) == false)
                    {
                        Console.WriteLine("     非退出指令,自动忽略...");
                    }
                    userCommand = Console.ReadLine();
                }
            }
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

            throw new ArgumentException("端口号含有非法字符");
        }
    }
}
