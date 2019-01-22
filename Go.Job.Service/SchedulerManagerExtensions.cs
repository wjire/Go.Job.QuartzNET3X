using System;
using System.Threading.Tasks;
using Go.Job.Service.Api;
using Go.Job.Service.Config;
using Go.Job.Service.Listener;
using Go.Job.Service.Logic;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Listener;

namespace Go.Job.Service
{
    public static class SchedulerManagerExtensions
    {

        public static async Task Start(this SchedulerManager manager)
        {
            await manager.Start(new DefaultJobListener(manager.Scheduler.SchedulerName));
        }


        public static async Task Start(this SchedulerManager manager, JobListenerSupport jobListener)
        {
            if (jobListener == null)
            {
                jobListener = new DefaultJobListener(manager.Scheduler.SchedulerName);
            }

            await manager.Start(jobListener, null);
        }

        public static async Task Start(this SchedulerManager manager, JobListenerSupport jobListener, TriggerListenerSupport triListener)
        {
            if (jobListener == null)
            {
                jobListener = new DefaultJobListener(manager.Scheduler.SchedulerName);
            }

            if (triListener == null)
            {
                triListener = new DefaultTriggerListener(manager.Scheduler.SchedulerName);
            }

            manager.Scheduler.ListenerManager.AddJobListener(jobListener, GroupMatcher<JobKey>.GroupEquals(manager.Scheduler.SchedulerName));
            manager.Scheduler.ListenerManager.AddTriggerListener(triListener, GroupMatcher<TriggerKey>.GroupEquals(manager.Scheduler.SchedulerName));
            await manager.Scheduler.Start();
            Console.WriteLine($"作业调度服务已启动! 当前调度任务 : {  manager.Scheduler.SchedulerName}");
            JobApiStartHelper.Start(AppSettingsConfig.ApiAddress, manager.Scheduler.SchedulerName);
        }
    }
}
