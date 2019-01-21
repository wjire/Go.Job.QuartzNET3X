using Quartz;
using Quartz.Listener;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Go.Job.Service.Listener
{
    public class BaseTriggerListener : TriggerListenerSupport
    {
        protected Action<IJobExecutionContext, ITrigger> FiredAction;

        protected Action<IJobExecutionContext, ITrigger> CompleteAction;

        protected Action<ITrigger> MisFiredAction;

        public override string Name { get; }

        protected BaseTriggerListener(string name) : this(name, null, null, null)
        {

        }

        protected BaseTriggerListener(string name, Action<IJobExecutionContext, ITrigger> firedAction, Action<IJobExecutionContext, ITrigger> completeAction, Action<ITrigger> misFiredAction)
        {
            Name = name;
            FiredAction = firedAction;
            CompleteAction = completeAction;
            MisFiredAction = misFiredAction;
        }

        public override Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                FiredAction(context, trigger);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return base.TriggerFired(trigger, context, cancellationToken);
        }

        public override Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                CompleteAction(context, trigger);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return base.TriggerComplete(trigger, context, triggerInstructionCode, cancellationToken);
        }

        public override Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                MisFiredAction?.Invoke(trigger);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return base.TriggerMisfired(trigger, cancellationToken);
        }

        public override Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.VetoJobExecution(trigger, context, cancellationToken);
        }
    }
}
