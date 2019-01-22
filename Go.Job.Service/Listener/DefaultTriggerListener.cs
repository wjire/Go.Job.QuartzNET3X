using Go.Job.Model;
using Go.Job.Service.Middleware;
using Quartz;
using System;

namespace Go.Job.Service.Listener
{
    public class DefaultTriggerListener : BaseTriggerListener
    {

        private static readonly ILogWriter LogWrite = (ILogWriter)MidContainer.GetService(typeof(ILogWriter));


        internal DefaultTriggerListener(string name) : base(name, InitFiredAction(), InitCompleteAction(), InitMisFiredAction())
        {

        }


        private static Action<IJobExecutionContext, ITrigger> InitFiredAction()
        {
            return (context, trigger) =>
            {
                JobInfo jobInfo = context.JobDetail.JobDataMap.Get("jobInfo") as JobInfo;
                if (jobInfo != null)
                {
                    LogWrite.SaveLog($"触发器监听 : ", $"{ DateTime.Now} : 触发器 { trigger.Key.Name} 开始点火");
                }
            };
        }


        private static Action<IJobExecutionContext, ITrigger> InitCompleteAction()
        {
            return (context, trigger) =>
            {
                JobInfo jobInfo = context.JobDetail.JobDataMap.Get("jobInfo") as JobInfo;
                if (jobInfo != null)
                {
                    LogWrite.SaveLog("触发器监听 : ", $"{DateTime.Now} : 触发器 {trigger.Key.Name} 点火完毕");
                }
            };
        }


        private static Action<ITrigger> InitMisFiredAction()
        {
            return (trigger) =>
            {
                JobInfo jobInfo = trigger.JobDataMap.Get("jobInfo") as JobInfo;
                if (jobInfo != null)
                {
                    LogWrite.SaveLog("触发器监听 : ", $"{DateTime.Now} : 触发器 {trigger.Key.Name} 哑火");
                }
            };
        }
    }
}
