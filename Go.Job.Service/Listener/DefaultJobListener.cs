using System;
using Go.Job.Model;
using Go.Job.Service.Middleware;
using Quartz;

namespace Go.Job.Service.Listener
{
    internal class DefaultJobListener : BaseJobListener
    {

        private static readonly ILogWriter LogWrite = (ILogWriter)MidContainer.GetService(typeof(ILogWriter));


        internal DefaultJobListener(string name) : base(name, InitStartAction(), InitEndAction())
        {

        }


        private static Action<IJobExecutionContext> InitStartAction()
        {
            return context =>
            {
                JobInfo jobInfo = context.JobDetail.JobDataMap.Get("jobInfo") as JobInfo;
                if (jobInfo != null)
                {
                    LogWrite.SaveLog("job开始执行", $"{DateTime.Now} : {jobInfo.JobName} 执行结束");
                }
            };
        }


        private static Action<IJobExecutionContext> InitEndAction()
        {
            return context =>
            {
                JobInfo jobInfo = context.JobDetail.JobDataMap.Get("jobInfo") as JobInfo;
                if (jobInfo != null)
                {
                    LogWrite.SaveLog("job执行结束", $"{DateTime.Now} : {jobInfo.JobName} 执行结束");
                }
            };
        }
    }
}
