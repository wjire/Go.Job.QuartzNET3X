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
    /// 
    /// </summary>
    public interface IJobService
    {

        string Start();
        void Start(JobInfo jobInfo);

        /// <summary>
        /// 添加 job 监听器
        /// </summary>
        /// <param name="jobListener"></param>
        /// <returns></returns>
        void AddJobListener(JobListenerSupport jobListener);
        

        /// <summary>
        /// 使用默认job监听器
        /// </summary>
        /// <param name="jobToBeExecutedAction">Job执行前</param>
        /// <param name="jobWasExecutedAction">Job执行后</param>
        /// <param name="jobExecutionVetoedAction">Job被拒绝时</param>
        /// <returns></returns>
        void UseDefaultJobListener(Action<IJobExecutionContext> jobToBeExecutedAction = null,
            Action<IJobExecutionContext> jobWasExecutedAction = null,
            Action<IJobExecutionContext> jobExecutionVetoedAction = null);



        /// <summary>
        /// 添加 trigger 监听器
        /// </summary>
        /// <param name="triggerListener"></param>
        /// <returns></returns>
        void AddTriggerListener(TriggerListenerSupport triggerListener);


        /// <summary>
        /// 使用默认trigger监听器
        /// </summary>
        /// <param name="firedAction">点火开始</param>
        /// <param name="completeAction">点火完成</param>
        /// <param name="misFiredAction">哑火</param>
        /// <param name="vetoJobAction">Job拒绝时</param>
        /// <returns></returns>
        void UseDefaultTriggerListener(Action<IJobExecutionContext, ITrigger> firedAction = null,
            Action<IJobExecutionContext, ITrigger> completeAction = null, Action<ITrigger> misFiredAction = null,
            Action<ITrigger> vetoJobAction = null);

    }
}
