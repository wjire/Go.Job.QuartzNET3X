using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Go.Job.Service.api;
using Go.Job.Service.Config;
using Quartz.Impl;

namespace Go.Job.Service
{

    /// <summary>
    /// 调度器启动类
    /// </summary>
    public static class SchedStartHelper
    {

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

        private static readonly string SchedName = AppSettingsConfig.SchedName;

        private static readonly string ApiAddress = AppSettingsConfig.ApiAddress;

        /// <summary>
        /// 启动调度器
        /// </summary>
        /// <returns></returns>
        public static void StartSched()
        {
            if (string.IsNullOrWhiteSpace(SchedName))
            {
                throw new ArgumentNullException("SchedName 不能为空,请在配置文件<appSettings>节点中设置 key=\"SchedName\" 的值");
            }

            if (string.IsNullOrWhiteSpace(ApiAddress))
            {
                throw new ArgumentNullException("ApiAddress 不能为空,请在配置文件<appSettings>节点中设置 key=\"ApiAddress\" 的值");
            }
            
            if (JobApiStartHelper.PortInUse(ApiAddress))
            {
                throw new ArgumentNullException($"{ApiAddress} 已被占用,请在配置文件<appSettings>节点中修改 key=\"ApiAddress\" 的值");
            }

            StartSched(SchedName, null, null);
        }


        /// <summary>
        /// 启动调度器
        /// </summary>
        /// <param name="schedName">调度器名称</param>
        /// <param name="poolConfig">线程池配置</param>
        /// <param name="storeConfig">持久化配置 </param>
        /// <returns></returns>
        private static void StartSched(string schedName, ThreadPoolConfig poolConfig = null, JobStoreConfig storeConfig = null)
        {
            if (storeConfig == null)
            {
                storeConfig = new JobStoreConfig();
            }

            if (poolConfig == null)
            {
                poolConfig = new ThreadPoolConfig();
            }

            storeConfig.InstanceName = schedName;
            StartSched(poolConfig, storeConfig).Wait();

            Console.WriteLine("作业调度服务已启动!");
            string userCommand = string.Empty;
            while (userCommand != "exit")
            {
                if (string.IsNullOrEmpty(userCommand) == false)
                {
                    Console.WriteLine("     非退出指令,自动忽略...");
                }

                JobApiStartHelper.Start();
                userCommand = Console.ReadLine();
            }
        }


        /// <summary>
        /// 启动调度器
        /// </summary>
        /// <param name="poolConfig">线程池配置</param>
        /// <param name="storeConfig">持久化配置 </param>
        /// <returns></returns>
        private static async Task StartSched(ThreadPoolConfig poolConfig, JobStoreConfig storeConfig)
        {
            NameValueCollection properties = new NameValueCollection { poolConfig, storeConfig };
            SchedulerManager.Scheduler = await new StdSchedulerFactory(properties).GetScheduler();
            try
            {
                if (!SchedulerManager.Scheduler.IsStarted)
                {
                    SchedulerManager.Scheduler.Start().Wait();
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
        private static async Task Run()
        {
            //设置监听器代码
            //SchedulerManager1.Scheduler.ListenerManager.AddJobListener(new MyJobListenerSupport("Job"), GroupMatcher<JobKey>.GroupEquals("Job"));
            //SchedulerManager1.Scheduler.ListenerManager.AddJobListener(new MyJobListenerSupport("Job2"), GroupMatcher<JobKey>.GroupEquals("Job2"));
        }
    }
}
