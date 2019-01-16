using System;
using System.Threading;
using System.Threading.Tasks;
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
            string name = context.JobDetail.Key.Name;
            Console.WriteLine($"{DateTime.Now} : {name} is Executed");
            if (Name == "ScanJob")
            {
                Console.WriteLine($"{DateTime.Now} : ScanJob is JobWasExecuted ");
                await context.Scheduler.PauseJob(new JobKey("ScanJob", "ScanJob"), cancellationToken);
                Console.WriteLine($"{DateTime.Now } :ScanJob is Paused");
            }

            await base.JobWasExecuted(context, jobException, cancellationToken);
        }

        public override async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.JobExecutionVetoed(context, cancellationToken);
        }

        public override Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            string name = context.JobDetail.Key.Name;
            Console.WriteLine($"{DateTime.Now} : {name} tobeExecuting ");
            return base.JobToBeExecuted(context, cancellationToken);
        }
    }
}
