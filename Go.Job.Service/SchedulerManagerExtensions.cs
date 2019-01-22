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
    /// <summary>
    /// 调度管理者扩展方法
    /// </summary>
    public static class SchedulerManagerExtensions
    {

        public static async Task Start(this SchedulerManager manager)
        {

            if (SchedulerConfig.JobListener!=null)
            {
                manager.Scheduler.ListenerManager.AddJobListener(SchedulerConfig.JobListener, GroupMatcher<JobKey>.GroupEquals(manager.Scheduler.SchedulerName));
            }

            if (SchedulerConfig.TriggerListener !=null)
            {
                manager.Scheduler.ListenerManager.AddTriggerListener(SchedulerConfig.TriggerListener, GroupMatcher<TriggerKey>.GroupEquals(manager.Scheduler.SchedulerName));
            }
            await manager.Scheduler.Start();
            Console.WriteLine($"作业调度服务已启动! 当前调度任务 : {  SchedulerConfig.SchedulerName}");
            JobApiStartHelper.Start(ApiConfig.ApiAddress, SchedulerConfig.SchedulerName);
        }


        /// <summary>
        /// 添加job监听器
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="useJobListener">false:不添加;true:添加</param>
        /// <param name="jobListener"></param>
        /// <returns></returns>
        public static SchedulerManager AddJobListener(this SchedulerManager manager, bool useJobListener, JobListenerSupport jobListener=null)
        {
            if (useJobListener ==false)
            {
                SchedulerConfig.JobListener = null;
                return manager;
            }
                return manager.AddJobListener(jobListener);
        }


        /// <summary>
        /// 添加job监听器
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="jobListener"></param>
        /// <returns></returns>
        private static SchedulerManager AddJobListener (this SchedulerManager manager, JobListenerSupport jobListener)
        {
            if (jobListener != null)
            {
                SchedulerConfig.JobListener = jobListener;
            }
            return manager;
        }

        /// <summary>
        /// 添加触发器监听器
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="useTriggerListener">false:不添加;true:添加</param>
        /// <param name="triggerListener"></param>
        /// <returns></returns>
        public static SchedulerManager AddTriggerListener(this SchedulerManager manager, bool useTriggerListener, TriggerListenerSupport triggerListener =null)
        {
            if (useTriggerListener == false)
            {
                SchedulerConfig.TriggerListener = null;
                return manager;
            }
            return manager.AddTriggerListener(triggerListener);
        }


        /// <summary>
        /// 添加触发器监听器
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="triggerListener"></param>
        /// <returns></returns>
        private static SchedulerManager AddTriggerListener(this SchedulerManager manager, TriggerListenerSupport triggerListener)
        {
            if (triggerListener != null)
            {
                SchedulerConfig.TriggerListener = triggerListener;
            }
            return manager;
        }
    }
}
