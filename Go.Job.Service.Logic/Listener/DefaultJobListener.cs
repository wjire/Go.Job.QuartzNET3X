using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Listener;

namespace Go.Job.Service.Logic.Listener
{
    /// <summary>
    /// job监听器基类
    /// </summary>
    public sealed class DefaultJobListener : JobListenerSupport
    {
        public Action<IJobExecutionContext> JobToBeExecutedAction;

        public Action<IJobExecutionContext> JobWasExecutedAction;

        public Action<IJobExecutionContext> JobExecutionVetoedAction;


        public override string Name { get; } = "Default";


        public override async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            JobWasExecutedAction?.Invoke(context);
            await base.JobWasExecuted(context, jobException, cancellationToken);
        }

        public override async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            JobExecutionVetoedAction?.Invoke(context);
            await base.JobExecutionVetoed(context, cancellationToken);
        }

        public override async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            JobToBeExecutedAction?.Invoke(context);
            await base.JobToBeExecuted(context, cancellationToken);
        }
    }
}
