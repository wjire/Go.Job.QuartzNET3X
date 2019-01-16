using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Go.Job.Service
{
    public class MyTriggerListener : ITriggerListener
    {
        public Task TriggerFired(ITrigger trigger, IJobExecutionContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public string Name { get; }
    }
}
