using System.Collections.Specialized;
using System.Threading.Tasks;
using Go.Job.Service.Config;
using Go.Job.Service.Lib;
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
            await CreateSchedulerAndStart(null);
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

            if (!JobPoolManager.Scheduler.IsStarted)
            {
                await JobPoolManager.Scheduler.Start();
            }

            //JobPoolManager.Scheduler.ListenerManager.AddJobListener(new MyJobListenerSupport("Job"), GroupMatcher<JobKey>.GroupEquals("Job"));

            //暂时注释掉 扫描job 代码
            //if (scanJobConfig != null)
            //{
            //    IJobDetail scanJobDetail = await JobPoolManager.Scheduler.GetJobDetail(new JobKey(JobString.ScanJob, JobString.ScanJob));
            //    if (scanJobDetail == null)
            //    {
            //        JobPoolManager.Scheduler.ListenerManager.AddJobListener(new MyJobListenerSupport("ScanJob"), KeyMatcher<JobKey>.KeyEquals(new JobKey("ScanJob", "ScanJob")));
            //        await ScanJobStartUp.StartScanJob(scanJobConfig);
            //    }
            //}
        }
    }
}
