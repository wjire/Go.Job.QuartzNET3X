using System;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Listener;

namespace Go.Job.Service.Listener
{
    public class MyTriggerListenerSupport : TriggerListenerSupport
    {
        public override string Name { get; }

        public override Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine(nameof(TriggerComplete));
            return base.TriggerComplete(trigger, context, triggerInstructionCode, cancellationToken);
        }

        public override Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine(nameof(TriggerFired));
            return base.TriggerFired(trigger, context, cancellationToken);
        }

        public override Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine(nameof(TriggerMisfired));
            return base.TriggerMisfired(trigger, cancellationToken);
        }

        public override Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine(nameof(VetoJobExecution));
            return base.VetoJobExecution(trigger, context, cancellationToken);
        }
    }
}
