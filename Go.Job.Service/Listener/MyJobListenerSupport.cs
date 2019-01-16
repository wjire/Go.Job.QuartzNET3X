using Quartz;
using Quartz.Listener;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Go.Job.Service
{
    public class MyJobListenerSupport : JobListenerSupport
    {
        public MyJobListenerSupport()
        {
            Name = "ScanJob";
        }

        public override string Name { get; }

        public override async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine($"{DateTime.Now} : ScanJob is JobWasExecuted ");
            await context.Scheduler.PauseJob(new JobKey("ScanJob", "ScanJob"), cancellationToken);
            Console.WriteLine($"{DateTime.Now } :ScanJob is Paused");
            await base.JobWasExecuted(context, jobException, cancellationToken);
        }

        public override async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine($"{DateTime.Now} : JobExecutionVetoed ");
            await base.JobExecutionVetoed(context, cancellationToken);
        }

        public override Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine($"{DateTime.Now} : ScanJob is JobToBeExecuted ");
            return base.JobToBeExecuted(context, cancellationToken);
        }
    }
}
