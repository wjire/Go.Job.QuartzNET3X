using Go.Job.Service.Config;
using Microsoft.Owin.Hosting;
using System;

namespace Go.Job.Service.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                //new SchedulerFactory(new SchedulerThreadPoolConfig(), new SchedulerRemoteExporterConfig()).CreateSchedulerAndStart().Wait();

                new SchedulerFactory(new SchedulerThreadPoolConfig(), new SchedulerRemoteExporterConfig(), new SchedulerJobStoreConfig()).CreateSchedulerAndStart().Wait();
                Console.WriteLine("作业调度服务已启动!");
                string userCommand = string.Empty;
                while (userCommand != "exit")
                {
                    if (string.IsNullOrEmpty(userCommand) == false)
                    {
                        Console.WriteLine("     非退出指令,自动忽略...");
                    }

                    //TODO:signalR 用不起,脑壳痛
                    //string url = "http://localhost:25111";
                    ////WebApp.Start(url); //啟動 SignalR Hub Server

                    ////WebApp.Start<Startup>(url);
                    //using (WebApp.Start(url))//啟動 SignalR Hub Server
                    //{
                    //    Console.WriteLine("Server running on {0}", url);
                    //    Console.ReadLine();
                    //}

                    //TODO:麻蛋,signalr 不行 用 webapi
                    {
                        string baseAddress = "http://localhost:25250/";
                        using (WebApp.Start(url: baseAddress))
                        {
                            Console.WriteLine("请开始您的表演");
                            Console.ReadLine();
                        }
                    }
                    
                    userCommand = Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }

            Console.ReadKey();

        }
    }
}
