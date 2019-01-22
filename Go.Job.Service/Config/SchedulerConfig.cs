using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Go.Job.Service.Listener;
using Quartz.Listener;

namespace Go.Job.Service.Config
{
    internal class SchedulerConfig
    {
        internal static string SchedulerName;

        internal static JobListenerSupport JobListener { get; set; } = new DefaultJobListener(SchedulerName);

        internal static TriggerListenerSupport TriggerListener { get; set; } = new DefaultTriggerListener(SchedulerName);


        private static JobListenerSupport CreateDefaultJobListener()
        {
            return new DefaultJobListener(SchedulerName);
        }

        private static TriggerListenerSupport CreateDefaultTriggerListener()
        {
            return new DefaultTriggerListener(SchedulerName);
        }
    }
}
