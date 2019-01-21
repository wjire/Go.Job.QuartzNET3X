using Quartz;
using Quartz.Listener;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Go.Job.Service.Listener
{
    public abstract class BaseJobListener : JobListenerSupport
    {
        protected Action<IJobExecutionContext> _startAction;

        protected Action<IJobExecutionContext> _endAction;

        public override string Name { get; }

        protected BaseJobListener(string name) : this(name, null, null)
        {

        }

        protected BaseJobListener(string name, Action<IJobExecutionContext> startAction, Action<IJobExecutionContext> endAction)
        {
            Name = name;
            _startAction = startAction;
            _endAction = endAction;
        }


        public override async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                _endAction?.Invoke(context);
                await base.JobWasExecuted(context, jobException, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
                _startAction?.Invoke(context);
                await base.JobToBeExecuted(context, cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
