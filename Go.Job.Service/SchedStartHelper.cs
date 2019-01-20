using System;
using System.Threading.Tasks;
using Go.Job.Service.api;
using Go.Job.Service.Config;
using Go.Job.Service.Core;
using Quartz.Impl;

namespace Go.Job.Service
{

    /// <summary>
    /// 调度器启动类
    /// </summary>
    public static class SchedStartHelper
    {

        /// <summary>
        /// 调度任务名称
        /// </summary>
        internal static string SchedName;

        /// <summary>
        /// 调度任务监听地址
        /// </summary>
        internal static readonly string ApiAddress = AppSettingsConfig.ApiAddress;

        /// <summary>
        /// 启动调度器
        /// </summary>
        /// <returns></returns>
        public static async Task StartSched()
        {
            try
            {
                SchedulerManager.Scheduler = await new StdSchedulerFactory().GetScheduler();
                SchedName = SchedulerManager.Scheduler.SchedulerName;
                //SchedulerManager.Scheduler.ListenerManager.AddJobListener(new MyJobListenerSupport(SchedName), GroupMatcher<JobKey>.GroupEquals(SchedName));
                if (!SchedulerManager.Scheduler.IsStarted)
                {
                    await SchedulerManager.Scheduler.Start();
                }

                Console.WriteLine($"作业调度服务已启动! 当前调度任务 : {SchedName}");
                JobApiStartHelper.Start(ApiAddress);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
        }


        //public async Task CreateSchedulerAndStart()
        //{
        //SchedulerManager1.Scheduler.ListenerManager.AddJobListener(new MyJobListenerSupport("Job"), GroupMatcher<JobKey>.GroupEquals("Job"));

        //暂时注释掉 扫描job 代码
        //if (scanJobConfig != null)
        //{
        //    IJobDetail scanJobDetail = await SchedulerManager1.Scheduler.GetJobDetail(new JobKey(JobString.ScanJob, JobString.ScanJob));
        //    if (scanJobDetail == null)
        //    {
        //        SchedulerManager1.Scheduler.ListenerManager.AddJobListener(new MyJobListenerSupport("ScanJob"), KeyMatcher<JobKey>.KeyEquals(new JobKey("ScanJob", "ScanJob")));
        //        await ScanJobStartUp.StartScanJob(scanJobConfig);
        //    }
        //}
        //}
    }
}
