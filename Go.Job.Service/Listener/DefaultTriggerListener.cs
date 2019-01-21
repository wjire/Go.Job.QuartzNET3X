using Go.Job.Model;
using Quartz;
using System;

namespace Go.Job.Service.Listener
{
    internal class DefaultTriggerListener : BaseTriggerListener
    {

        private static readonly ILogWriter LogWrite = (ILogWriter)ServiceContainer.GetService(typeof(ILogWriter));


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
                    //LogWrite.WriteLogAfterEnd(new JobLog { JobInfo = jobInfo });
                    Console.WriteLine($"trigger 监听器 : {DateTime.Now} - {jobInfo.JobName} 开始执行!");
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
                    //LogWrite.WriteLogBeforeStart(new JobLog { JobInfo = jobInfo });
                    Console.WriteLine($"trigger 监听器 : {DateTime.Now} - {jobInfo.JobName} 执行结束!");

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
                    LogWrite.WriteLogBeforeStart(new JobLog { JobInfo = jobInfo });
                }
            };
        }
    }
}
