using System;
using Microsoft.Owin.Hosting;

namespace Go.Job.Service.api
{
    /// <summary>
    /// jobApi启动类
    /// </summary>
    public class JobApiStartHelper
    {
        private static readonly string ApiAddress = System.Configuration.ConfigurationManager.AppSettings["ApiAddress"];

        public static void Start()
        {
            if (string.IsNullOrWhiteSpace(ApiAddress))
            {
                throw new ArgumentNullException("ApiAddress 地址不能为空,请在配置文件<appSettings>节点中设置 key=\"ApiAddress\" 的值,或者调用该方法的重载版本");
            }

            Start(ApiAddress);
        }


        public static void Start(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentNullException("address 不能为空");
            }

            using (WebApp.Start(address))
            {
                Console.WriteLine($"webapi监听已启动, address : {address}");
                Console.ReadLine();
            }
        }
    }
}
