using Go.Job.Service.Config;
using Go.Job.Service.Job;
using Go.Job.Service.Lib;
using Quartz;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Go.Job.Service.Helper
{
    public static class ScanJobStartUp
    {
        public static async Task StartScanJob(ScanJobConfig scanJobConfig)
        {
            //创建扫描Job
            IJobDetail jobDetail = JobBuilder.Create<ScanJob>()
                .WithIdentity(JobString.ScanJob, JobString.ScanJob)
                .Build();
            TriggerBuilder triggerBuilder = TriggerBuilder.Create()
                .WithIdentity(JobString.ScanJob, JobString.ScanJob);

            if (scanJobConfig.Type == ScanJobIntervalType.分)
            {
                triggerBuilder.WithSimpleSchedule(s =>
                        s.WithIntervalInMinutes(scanJobConfig.IntervalTime).RepeatForever().WithMisfireHandlingInstructionFireNow())
                    .StartNow();
            }
            if (scanJobConfig.Type == ScanJobIntervalType.秒)
            {
                triggerBuilder.WithSimpleSchedule(s =>
                        s.WithIntervalInSeconds(scanJobConfig.IntervalTime).RepeatForever().WithMisfireHandlingInstructionFireNow())
                    .StartNow();
                //.StartAt(new DateTimeOffset(DateTime.Now.AddSeconds(5)))
            }

            ITrigger trigger = triggerBuilder.Build();
            await JobPoolManager.Scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}