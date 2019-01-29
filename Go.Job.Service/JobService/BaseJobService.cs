using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Go.Job.Model;
using Go.Job.Service.Logic;
using Go.Job.Service.Logic.Listener;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Listener;

namespace Go.Job.Service
{
    /// <summary>
    /// 调度服务基类
    /// </summary>
    public abstract class BaseJobService :IJobService
    {
        public SchedulerManager Manager { get; set; }

        public abstract string Start();

        public virtual void Start(JobInfo jobInfo)
        {
            Start();
        }

        /// <summary>
        /// 添加 job 监听器
        /// </summary>
        /// <param name="jobListener"></param>
        /// <returns></returns>
        public void AddJobListener(JobListenerSupport jobListener)
        {
            if (jobListener != null)
            {
                Manager.Scheduler.ListenerManager.AddJobListener(jobListener, GroupMatcher<JobKey>.GroupEquals(Manager.Scheduler.SchedulerName));
            }
        }


        /// <summary>
        /// 使用默认job监听器
        /// </summary>
        /// <param name="jobToBeExecutedAction">Job执行前</param>
        /// <param name="jobWasExecutedAction">Job执行后</param>
        /// <param name="jobExecutionVetoedAction">Job被拒绝时</param>
        /// <returns></returns>
        public void UseDefaultJobListener(Action<IJobExecutionContext> jobToBeExecutedAction = null, Action<IJobExecutionContext> jobWasExecutedAction = null, Action<IJobExecutionContext> jobExecutionVetoedAction = null)
        {
            DefaultJobListener listener = new DefaultJobListener
            {
                JobExecutionVetoedAction = jobExecutionVetoedAction,
                JobToBeExecutedAction = jobToBeExecutedAction,
                JobWasExecutedAction = jobWasExecutedAction,
            };
            Manager.Scheduler.ListenerManager.AddJobListener(listener, GroupMatcher<JobKey>.GroupEquals(Manager.Scheduler.SchedulerName));
        }


        /// <summary>
        /// 添加 trigger 监听器
        /// </summary>
        /// <param name="triggerListener"></param>
        /// <returns></returns>
        public void AddTriggerListener(TriggerListenerSupport triggerListener)
        {
            if (triggerListener != null)
            {
                Manager.Scheduler.ListenerManager.AddTriggerListener(triggerListener, GroupMatcher<TriggerKey>.GroupEquals(Manager.Scheduler.SchedulerName));
            }
        }

        /// <summary>
        /// 使用默认trigger监听器
        /// </summary>
        /// <param name="firedAction">点火开始</param>
        /// <param name="completeAction">点火完成</param>
        /// <param name="misFiredAction">哑火</param>
        /// <param name="vetoJobAction">Job拒绝时</param>
        /// <returns></returns>
        public void UseDefaultTriggerListener(Action<IJobExecutionContext, ITrigger> firedAction = null, Action<IJobExecutionContext, ITrigger> completeAction = null, Action<ITrigger> misFiredAction = null, Action<ITrigger> vetoJobAction = null)
        {
            DefaultTriggerListener listener = new DefaultTriggerListener
            {
                CompleteAction = completeAction,
                FiredAction = firedAction,
                MisFiredAction = misFiredAction,
                VetoJobAction = vetoJobAction
            };
            Manager.Scheduler.ListenerManager.AddTriggerListener(listener, GroupMatcher<TriggerKey>.GroupEquals(Manager.Scheduler.SchedulerName));
        }
    }
}
