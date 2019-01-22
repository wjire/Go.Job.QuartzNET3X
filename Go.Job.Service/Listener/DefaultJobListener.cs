using Go.Job.Model;
using Go.Job.Service.Middleware;
using Quartz;
using System;

namespace Go.Job.Service.Listener
{
    public class DefaultJobListener : BaseJobListener
    {

        internal DefaultJobListener(string name) : base(name)
        {
            StartAction = InitStartAction();
            EndAction = InitEndAction();
        }


        private Action<IJobExecutionContext> InitStartAction()
        {
            return context =>
            {
                JobInfo jobInfo = context.JobDetail.JobDataMap.Get("jobInfo") as JobInfo;
                if (jobInfo != null)
                {
                    LogWriter.SaveLog("job监听器 : ", $"{DateTime.Now} : {jobInfo.JobName} 开始执行");
                }
            };
        }


        private Action<IJobExecutionContext> InitEndAction()
        {
            return context =>
            {
                JobInfo jobInfo = context.JobDetail.JobDataMap.Get("jobInfo") as JobInfo;
                if (jobInfo != null)
                {
                    LogWriter.SaveLog("job监听器", $"{DateTime.Now} : {jobInfo.JobName} 执行结束");
                }
            };
        }
    }
}
