using Go.Job.Service.Config;
using Go.Job.Service.WebAPI;
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

                new SchedulerFactory(new SchedulerThreadPoolConfig(), null, new SchedulerJobStoreConfig()).CreateSchedulerAndStart().Wait();
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
    }
}
