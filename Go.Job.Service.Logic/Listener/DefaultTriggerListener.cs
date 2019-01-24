using Go.Job.Model;
using Go.Job.Service.Middleware;
using Quartz;
using System;

namespace Go.Job.Service.Logic.Listener
{
    public class DefaultTriggerListener : BaseTriggerListener
    {

        public DefaultTriggerListener(string name) : base(name)
        {
            FiredAction = InitFiredAction();
            CompleteAction = InitCompleteAction();
            MisFiredAction = InitMisFiredAction();
        }


        private Action<IJobExecutionContext, ITrigger> InitFiredAction()
        {
            return (context, trigger) =>
            {
                JobInfo jobInfo = context.JobDetail.JobDataMap.Get("jobInfo") as JobInfo;
                if (jobInfo != null)
                {
                    LogWriter.WriteLog($"{ DateTime.Now} : 触发器 { trigger.Key.Name} 开始点火",jobInfo.JobName);
                }
            };
        }


        private Action<IJobExecutionContext, ITrigger> InitCompleteAction()
        {
            return (context, trigger) =>
            {
                JobInfo jobInfo = context.JobDetail.JobDataMap.Get("jobInfo") as JobInfo;
                if (jobInfo != null)
                {
                    LogWriter.WriteLog($"{DateTime.Now} : 触发器 {trigger.Key.Name} 点火完毕",jobInfo.JobName);
                }
            };
        }


        private Action<ITrigger> InitMisFiredAction()
        {
            return (trigger) =>
            {
                JobInfo jobInfo = trigger.JobDataMap.Get("jobInfo") as JobInfo;
                if (jobInfo != null)
                {
                    LogWriter.WriteLog($"{DateTime.Now} : 触发器 {trigger.Key.Name} 哑火",jobInfo.JobName);
                }
            };
        }
    }
}
