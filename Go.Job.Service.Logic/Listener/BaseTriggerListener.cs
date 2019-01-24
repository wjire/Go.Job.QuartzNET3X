using Quartz;
using Quartz.Listener;
using System;
using System.Threading;
using System.Threading.Tasks;
using Go.Job.Service.Middleware;

namespace Go.Job.Service.Logic.Listener
{
    /// <summary>
    /// 触发器监听器基类
    /// </summary>
    public abstract class BaseTriggerListener : TriggerListenerSupport
    {

        protected readonly ILogWriter LogWriter = (ILogWriter)MidContainer.GetService(typeof(ILogWriter));

        protected Action<IJobExecutionContext, ITrigger> FiredAction;

        protected Action<IJobExecutionContext, ITrigger> CompleteAction;

        protected Action<ITrigger> MisFiredAction;

        public override string Name { get; }

        protected BaseTriggerListener(string name) 
        {
            Name = name;
        }

        public override Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                FiredAction?.Invoke(context, trigger);
            }
            catch (Exception e)
            {
                LogWriter.WriteException(e, trigger.Key.Name + ":" + nameof(TriggerFired));
            }
            return base.TriggerFired(trigger, context, cancellationToken);
        }

        public override Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                CompleteAction?.Invoke(context, trigger);
            }
            catch (Exception e)
            {
                LogWriter.WriteException(e, trigger.Key.Name + ":" + nameof(TriggerComplete));
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
                LogWriter.WriteException(e, trigger.Key.Name + ":" + nameof(TriggerMisfired));
            }
            return base.TriggerMisfired(trigger, cancellationToken);
        }

        public override Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.VetoJobExecution(trigger, context, cancellationToken);
        }
    }
}
