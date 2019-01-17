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
            //string name = context.JobDetail.Key.Name;
            //Console.WriteLine($"{DateTime.Now} : {name} is Executed");
            if (Name == "ScanJob")
            {
                var keys = context.JobDetail.JobDataMap.Keys;
                foreach (var key in keys)
                {
                    Console.WriteLine(key + " : " + context.JobDetail.JobDataMap.GetInt(key));
                }
                Console.WriteLine($"{DateTime.Now} : ScanJob is JobWasExecuted ");
                await context.Scheduler.PauseJob(new JobKey("ScanJob", "ScanJob"), cancellationToken);
                Console.WriteLine($"{DateTime.Now} :ScanJob is Paused");
                Console.WriteLine();
            }

            await base.JobWasExecuted(context, jobException, cancellationToken);
        }

        public override async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.JobExecutionVetoed(context, cancellationToken);
        }

        public override async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (Name == "ScanJob")
            {
                foreach (var key in context.JobDetail.JobDataMap.Keys)
                {
                    Console.WriteLine(key + " : " + context.JobDetail.JobDataMap[key]);
                }
                Console.WriteLine();
            }
            await base.JobToBeExecuted(context, cancellationToken);
        }
    }
}
