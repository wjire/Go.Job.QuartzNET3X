using System;
using System.Threading.Tasks;
using Go.Job.Service.Api;
using Go.Job.Service.Logic;
using Go.Job.Service.Logic.Listener;
using Go.Job.Service.Middleware;
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
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public static void Start(this SchedulerManager manager)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(manager.Scheduler.SchedulerName))
                {
                    throw new ArgumentNullException("调度任务名称不能为空,请前往配置文件修改!");
                }
                
                if (!manager.Scheduler.IsStarted)
                {
                    manager.Scheduler.Start().Wait();
                }
                Console.WriteLine($"作业调度服务已启动! 当前调度任务 : { manager.Scheduler.SchedulerName}");
                JobApiStartHelper.Start(manager.Scheduler.SchedulerName);
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


        /// <summary>
        /// 添加 job 监听器
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="jobListener"></param>
        /// <returns></returns>
        public static SchedulerManager AddJobListener(this SchedulerManager manager, JobListenerSupport jobListener)
        {
            if (jobListener != null)
            {
                manager.Scheduler.ListenerManager.AddJobListener(jobListener, GroupMatcher<JobKey>.GroupEquals(manager.Scheduler.SchedulerName));
            }
            return manager;
        }


        /// <summary>
        /// 使用默认job监听器
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="jobToBeExecutedAction">Job执行前</param>
        /// <param name="jobWasExecutedAction">Job执行后</param>
        /// <param name="jobExecutionVetoedAction">Job被拒绝时</param>
        /// <returns></returns>
        public static SchedulerManager UseDefaultJobListener(this SchedulerManager manager, Action<IJobExecutionContext> jobToBeExecutedAction = null, Action<IJobExecutionContext> jobWasExecutedAction = null, Action<IJobExecutionContext> jobExecutionVetoedAction = null)
        {
            DefaultJobListener listener = new DefaultJobListener
            {
                JobExecutionVetoedAction = jobExecutionVetoedAction,
                JobToBeExecutedAction = jobToBeExecutedAction,
                JobWasExecutedAction = jobWasExecutedAction,
            };
            manager.Scheduler.ListenerManager.AddJobListener(listener, GroupMatcher<JobKey>.GroupEquals(manager.Scheduler.SchedulerName));
            return manager;
        }


        /// <summary>
        /// 添加 trigger 监听器
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="triggerListener"></param>
        /// <returns></returns>
        public static SchedulerManager AddTriggerListener(this SchedulerManager manager, TriggerListenerSupport triggerListener)
        {
            if (triggerListener != null)
            {
                manager.Scheduler.ListenerManager.AddTriggerListener(triggerListener, GroupMatcher<TriggerKey>.GroupEquals(manager.Scheduler.SchedulerName));
            }
            return manager;
        }

        /// <summary>
        /// 使用默认trigger监听器
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="firedAction">点火开始</param>
        /// <param name="completeAction">点火完成</param>
        /// <param name="misFiredAction">哑火</param>
        /// <param name="vetoJobAction">Job拒绝时</param>
        /// <returns></returns>
        public static SchedulerManager UseDefaultTriggerListener(this SchedulerManager manager, Action<IJobExecutionContext, ITrigger> firedAction = null, Action<IJobExecutionContext, ITrigger> completeAction = null, Action<ITrigger> misFiredAction = null, Action<ITrigger> vetoJobAction = null)
        {
            DefaultTriggerListener listener = new DefaultTriggerListener
            {
                CompleteAction = completeAction,
                FiredAction = firedAction,
                MisFiredAction = misFiredAction,
                VetoJobAction = vetoJobAction
            };
            manager.Scheduler.ListenerManager.AddTriggerListener(listener, GroupMatcher<TriggerKey>.GroupEquals(manager.Scheduler.SchedulerName));
            return manager;
        }
    }
}
