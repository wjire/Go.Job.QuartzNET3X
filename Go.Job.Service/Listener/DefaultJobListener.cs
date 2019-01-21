using Go.Job.Model;
using Quartz;
using System;

namespace Go.Job.Service.Listener
{
    internal class DefaultJobListener : BaseJobListener
    {

        private static readonly ILogWriter LogWrite = (ILogWriter)ServiceContainer.GetService(typeof(ILogWriter));


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
                    //LogWrite.WriteLogAfterEnd(new JobLog { JobInfo = jobInfo });
                    Console.WriteLine($"job 监听器 : {DateTime.Now} - {jobInfo.JobName} 开始执行!");
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
                    //LogWrite.WriteLogBeforeStart(new JobLog { JobInfo = jobInfo });
                    Console.WriteLine($"job 监听器 : {DateTime.Now} - {jobInfo.JobName} 执行结束!");
                }
            };
        }
    }
}
