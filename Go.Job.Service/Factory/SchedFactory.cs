using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Go.Job.Service.Config;
using Quartz.Impl;

namespace Go.Job.Service
{

    /// <summary>
    /// 调度器启动类
    /// </summary>
    public static class SchedulerStartup
    {
        public static readonly NameValueCollection _properties = new NameValueCollection();


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


        /// <summary>
        /// 启动调度器
        /// </summary>
        /// <param name="schedName">调度器名称</param>
        /// <param name="poolConfig">线程池配置</param>
        /// <param name="storeConfig">持久化配置 </param>
        /// <returns></returns>
        public static async Task StartSched(string schedName, ThreadPoolConfig poolConfig = null, JobStoreConfig storeConfig = null)
        {
            if (string.IsNullOrWhiteSpace(schedName))
            {
                throw new ArgumentNullException("调度器名称不能为空");
            }

            if (storeConfig == null)
            {
                storeConfig = new JobStoreConfig();
            }

            if (poolConfig == null)
            {
                poolConfig = new ThreadPoolConfig();
            }

            storeConfig.InstanceName = schedName;
            await StartSched(poolConfig, storeConfig);
        }


        /// <summary>
        /// 启动调度器
        /// </summary>
        /// <param name="poolConfig">线程池配置</param>
        /// <param name="storeConfig">持久化配置 </param>
        /// <returns></returns>
        private static async Task StartSched(ThreadPoolConfig poolConfig, JobStoreConfig storeConfig)
        {
            _properties.Add(poolConfig);
            _properties.Add(storeConfig);
            SchedulerManager.Scheduler = await new StdSchedulerFactory(_properties).GetScheduler(storeConfig.InstanceName);
            try
            {
                if (!SchedulerManager.Scheduler.IsStarted)
                {
                    await SchedulerManager.Scheduler.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// 启动调度器
        /// </summary>
        /// <returns></returns>
        public static async Task Run()
        {
            //设置监听器代码
            //SchedulerManager1.Scheduler.ListenerManager.AddJobListener(new MyJobListenerSupport("Job"), GroupMatcher<JobKey>.GroupEquals("Job"));
            //SchedulerManager1.Scheduler.ListenerManager.AddJobListener(new MyJobListenerSupport("Job2"), GroupMatcher<JobKey>.GroupEquals("Job2"));
        }
    }
}
