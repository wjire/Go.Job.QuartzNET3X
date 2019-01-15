using System.Collections.Specialized;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace Go.Job.Service
{
    public sealed class SchedulerFactory
    {
        private readonly NameValueCollection _properties;


        public SchedulerFactory() : this(null)
        {

        }

        public SchedulerFactory(NameValueCollection properties)
        {
            _properties = properties;
        }

        public async Task CreateSchedulerAndStart()
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

            //创建扫描Job
            IJobDetail jobDetail = JobBuilder.Create<ScanJob>().WithIdentity("ScanJob", "ScanJob").Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("ScanJob", "ScanJob")
                .WithSimpleSchedule(s => s.WithIntervalInSeconds(10).RepeatForever())
                .StartNow()
                //.StartAt(new DateTimeOffset(DateTime.Now.AddSeconds(5)))
                .Build();
            await JobPoolManager.Scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}
