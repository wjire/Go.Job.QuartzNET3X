using System;
using System.Net;
using System.Net.NetworkInformation;
using Go.Job.Service.Config;
using Go.Job.Service.WebAPI;

namespace Go.Job.Service.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {

                //Console.WriteLine("Port: 555 status: " + (PortInUse(25250) ? "in use" : "not in use"));
                //Console.ReadKey();
                //new SchedFactory().AddThreadPoolConfig().AddRemoteConfig().AddJobStoreConfig().Run().Wait();

                SchedulerStartup.StartSched("wechat").Wait();

                Console.WriteLine("作业调度服务已启动!");
                string userCommand = string.Empty;
                while (userCommand != "exit")
                {
                    if (string.IsNullOrEmpty(userCommand) == false)
                    {
                        Console.WriteLine("     非退出指令,自动忽略...");
                    }

                    string address = "http://localhost:25251/";
                    WebApiHelper.Start(address);
                    userCommand = Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();

        }

        public static bool PortInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }
    }
}
