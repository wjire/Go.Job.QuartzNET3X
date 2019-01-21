using Go.Job.Service.api;
using Go.Job.Service.Config;
using Go.Job.Service.Core;
using Go.Job.Service.Listener;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Threading.Tasks;

namespace Go.Job.Service
{

    /// <summary>
    /// 调度器启动类
    /// </summary>
    public static class SchedulerManagerFacotry
    {

        /// <summary>
        /// 调度任务监听地址
        /// </summary>
        internal static readonly string ApiAddress = AppSettingsConfig.ApiAddress;


        public static SchedulerManager CreateSchedulerManager()
        {
            SchedulerManager.Singleton.Scheduler = new StdSchedulerFactory().GetScheduler().Result;
            return SchedulerManager.Singleton;
        }

        

        //public async Task CreateSchedulerAndStart()
        //{
        //SchedulerManager1.Scheduler.ListenerManager.AddJobListener(new DefaultJobListenerSupport("Job"), GroupMatcher<JobKey>.GroupEquals("Job"));

        //暂时注释掉 扫描job 代码
        //if (scanJobConfig != null)
        //{
        //    IJobDetail scanJobDetail = await SchedulerManager1.Scheduler.GetJobDetail(new JobKey(JobString.ScanJob, JobString.ScanJob));
        //    if (scanJobDetail == null)
        //    {
        //        SchedulerManager1.Scheduler.ListenerManager.AddJobListener(new DefaultJobListenerSupport("ScanJob"), KeyMatcher<JobKey>.KeyEquals(new JobKey("ScanJob", "ScanJob")));
        //        await ScanJobStartUp.StartScanJob(scanJobConfig);
        //    }
        //}
        //}
    }
}
