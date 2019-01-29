using Go.Job.Service.Logic;
using Quartz.Impl;
using System;

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
        public override string Start()
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
                return Manager.Scheduler.SchedulerName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }
    }
}
