using System.Collections.Specialized;
using System.Threading.Tasks;
using Go.Job.Service.Config;
using Go.Job.Service.Helper;
using Go.Job.Service.Listener;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace Go.Job.Service
{
    public sealed class SchedulerFactory
    {
        private readonly NameValueCollection _properties = new NameValueCollection();

        public SchedulerFactory() : this(null)
        {
        }

        public SchedulerFactory(SchedulerThreadPoolConfig threadPoolConfig) : this(threadPoolConfig, null)
        {
        }

        public SchedulerFactory(SchedulerThreadPoolConfig threadPoolConfig, SchedulerRemoteExporterConfig remoteExporterConfig) : this(threadPoolConfig, remoteExporterConfig, null)
        {
        }

        public SchedulerFactory(SchedulerThreadPoolConfig threadPoolConfig, SchedulerRemoteExporterConfig remoteExporterConfig, SchedulerJobStoreConfig jobStoreConfig)
        {
            if (threadPoolConfig != null)
            {
                _properties.Add(threadPoolConfig.Properties);
            }

            if (remoteExporterConfig != null)
            {
                _properties.Add(remoteExporterConfig.Properties);
            }

            if (jobStoreConfig != null)
            {
                _properties.Add(jobStoreConfig.Properties);
            }
        }

        public async Task CreateSchedulerAndStart()
        {
            await CreateSchedulerAndStart(new ScanJobConfig());
        }

        public async Task CreateSchedulerAndStart(ScanJobConfig scanJobConfig)
        {

            if (_properties == null)
            {
                JobPoolManager.Scheduler = await new StdSchedulerFactory().GetScheduler();
            }
            else
            {
                JobPoolManager.Scheduler = await new StdSchedulerFactory(_properties).GetScheduler();
            }
            await JobPoolManager.Scheduler.Start();

            JobPoolManager.Scheduler.ListenerManager.AddJobListener(new MyJobListenerSupport("ScanJob"), KeyMatcher<JobKey>.KeyEquals(new JobKey("ScanJob", "ScanJob")));
            JobPoolManager.Scheduler.ListenerManager.AddJobListener(new MyJobListenerSupport("Job"), GroupMatcher<JobKey>.GroupEquals("Job"));
            JobPoolManager.Scheduler.ListenerManager.AddJobListener(new MyJobListenerSupport("Job2"), GroupMatcher<JobKey>.GroupEquals("Job2"));

            await ScanJobStartUp.StartScanJob(scanJobConfig);
        }
    }
}
