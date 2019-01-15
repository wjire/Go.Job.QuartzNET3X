using Go.Job.Service;
using System;
using System.Collections.Specialized;

namespace Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            NameValueCollection properties = new NameValueCollection();

            properties["quartz.scheduler.instanceName"] = "调度作业监控系统";

            //设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";
            //设置配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "555";
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp";
            properties["quartz.scheduler.exporter.channelName"] = "httpQuartz";
            properties["quartz.scheduler.exporter.rejectRemoteRequests"] = "true";

            new SchedulerFactory(properties).CreateSchedulerAndStart().Wait();

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
