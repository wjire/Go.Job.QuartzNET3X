using Quartz;
using Quartz.Listener;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Go.Job.Service.Listener
{
    public abstract class BaseJobListener : JobListenerSupport
    {
        protected Action<IJobExecutionContext> StartAction;

        protected Action<IJobExecutionContext> EndAction;

        public override string Name { get; }

        protected BaseJobListener(string name) : this(name, null, null)
        {

        }

        protected BaseJobListener(string name, Action<IJobExecutionContext> startAction, Action<IJobExecutionContext> endAction)
        {
            Name = name;
            StartAction = startAction;
            EndAction = endAction;
        }


        public override async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                EndAction?.Invoke(context);
                await base.JobWasExecuted(context, jobException, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.JobExecutionVetoed(context, cancellationToken);
        }

        public override async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                StartAction?.Invoke(context);
                await base.JobToBeExecuted(context, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
