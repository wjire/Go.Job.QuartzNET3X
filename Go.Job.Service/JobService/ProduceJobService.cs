using Go.Job.Service.Api;
using Go.Job.Service.Logic;
using Quartz.Impl;
using System;
using Go.Job.Service.Logic.Listener;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Listener;

namespace Go.Job.Service
{
    /// <summary>
    /// 生成环境调度服务
    /// </summary>
    public class ProduceJobService : BaseJobService
    {
        /// <summary>
        /// 
        /// </summary>
        public ProduceJobService()
        {
            Manager = SchedulerManager.Singleton;
            SchedulerManager.Singleton.Scheduler = new StdSchedulerFactory().GetScheduler().Result;
        }
        

        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        public override void Start()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Manager.Scheduler.SchedulerName))
                {
                    throw new ArgumentNullException("调度任务名称不能为空,请前往配置文件修改!");
                }

                if (!Manager.Scheduler.IsStarted)
                {
                    Manager.Scheduler.Start().Wait();
                }
                Console.WriteLine($"作业调度服务已启动! 当前调度器为 : { Manager.Scheduler.SchedulerName}");
                JobApiStartHelper.Start(Manager.Scheduler.SchedulerName);
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}
