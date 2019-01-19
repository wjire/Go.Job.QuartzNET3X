using System;
using System.Threading;
using System.Threading.Tasks;
using Go.Job.Model;
using Quartz;
using Quartz.Listener;

namespace Go.Job.Service.Listener
{
    public class MyJobListenerSupport : JobListenerSupport
    {
        public MyJobListenerSupport(string name)
        {
            Name = name;
        }

        public override string Name { get; }

        public override async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            JobInfo jobInfo = context.JobDetail.JobDataMap.Get("jobInfo") as JobInfo ?? new JobInfo();
            Console.WriteLine($"{DateTime.Now} . {jobInfo.SchedName} : {jobInfo.JobName}");
            await base.JobWasExecuted(context, jobException, cancellationToken);
        }

        public override async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.JobExecutionVetoed(context, cancellationToken);
        }

        public override async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.JobToBeExecuted(context, cancellationToken);
        }
    }
}
