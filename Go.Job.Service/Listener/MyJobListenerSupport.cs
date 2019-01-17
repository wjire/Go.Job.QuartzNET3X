using Quartz;
using Quartz.Listener;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            Console.WriteLine($"{DateTime.Now} : {Name} is JobWasExecuted ");
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
