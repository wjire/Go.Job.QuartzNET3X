using Go.Job.Service.api;
using Go.Job.Service.Config;
using Go.Job.Service.Core;
using Go.Job.Service.Listener;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Listener;
using System;
using System.Threading.Tasks;

namespace Go.Job.Service
{
    public static class SchedulerManagerExtensions
    {

        public static async Task Start(this SchedulerManager manager)
        {
            await manager.Start(new DefaultJobListener(manager.Scheduler.SchedulerName));
        }


        public static async Task Start(this SchedulerManager manager, JobListenerSupport listener)
        {
            if (listener == null)
            {
                listener = new DefaultJobListener(manager.Scheduler.SchedulerName);
            }
            manager.Scheduler.ListenerManager.AddJobListener(listener, GroupMatcher<JobKey>.GroupEquals(manager.Scheduler.SchedulerName));
            await manager.Scheduler.Start();
            Console.WriteLine($"作业调度服务已启动! 当前调度任务 : {  manager.Scheduler.SchedulerName}");
            JobApiStartHelper.Start(AppSettingsConfig.ApiAddress);
        }


        //public static IScheduler AddListener(this SchedulerManager manager, JobListenerSupport listener)
        //{
        //    if (listener == null)
        //    {
        //        throw new ArgumentNullException(nameof(listener));
        //    }
        //    manager.Scheduler.ListenerManager.AddJobListener(listener, GroupMatcher<JobKey>.GroupEquals(manager.Scheduler.SchedulerName));
        //    return manager.Scheduler;
        //}
    }
}
