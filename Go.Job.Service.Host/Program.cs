using Go.Job.Service.Config;
using System;

namespace Go.Job.Service.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new SchedulerFactory(new SchedulerThreadPoolConfig(), new SchedulerRemoteExporterConfig()).CreateSchedulerAndStart().Wait();
            //new SchedulerFactory(new SchedulerThreadPoolConfig(), new SchedulerRemoteExporterConfig(), new SchedulerJobStoreConfig()).CreateSchedulerAndStart().Wait();

            Console.WriteLine("作业调度服务已启动!");
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
}
