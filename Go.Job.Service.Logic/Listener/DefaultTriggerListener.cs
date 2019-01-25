using System;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Listener;

namespace Go.Job.Service.Logic.Listener
{
    /// <summary>
    /// 触发器监听器基类
    /// </summary>
    public sealed class DefaultTriggerListener : TriggerListenerSupport
    {

        public Action<IJobExecutionContext, ITrigger> FiredAction;

        public Action<IJobExecutionContext, ITrigger> CompleteAction;

        public Action<ITrigger> MisFiredAction;

        public Action<ITrigger> VetoJobAction;

        public override string Name { get; } = "Default";


        public override Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {

            FiredAction?.Invoke(context, trigger);
            return base.TriggerFired(trigger, context, cancellationToken);
        }

        public override Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default(CancellationToken))
        {

            CompleteAction?.Invoke(context, trigger);

            return base.TriggerComplete(trigger, context, triggerInstructionCode, cancellationToken);
        }

        public override Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {

            MisFiredAction?.Invoke(trigger);

            return base.TriggerMisfired(trigger, cancellationToken);
        }

        public override Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {

            VetoJobAction?.Invoke(trigger);

            return base.VetoJobExecution(trigger, context, cancellationToken);
        }
    }
}
