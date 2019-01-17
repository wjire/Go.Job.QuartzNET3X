using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Listener;

namespace Go.Job.Service.Listener
{
    class Class1 : JobListenerSupport
    {
        public override string Name { get; }
        public override Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.JobToBeExecuted(context, cancellationToken);
        }
    }
}
