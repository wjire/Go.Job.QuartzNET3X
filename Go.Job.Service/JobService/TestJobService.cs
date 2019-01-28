using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Go.Job.Model;
using Go.Job.Service.Api;
using Go.Job.Service.Config;
using Go.Job.Service.Logic;
using Quartz;
using Quartz.Impl;

namespace Go.Job.Service
{
    /// <summary>
    /// 测试
    /// </summary>
    public class TestJobService: BaseJobService
    {
       
        /// <summary>
        /// 
        /// </summary>
        public TestJobService()
        {
            Manager = SchedulerManager.Singleton;
            Manager.Scheduler = new StdSchedulerFactory(new ThreadPoolConfig()).GetScheduler().Result;
        }
        
        /// <summary>
        /// 启动
        /// </summary>
        public  override void Start()
        {
            try
            {
                if (!Manager.Scheduler.IsStarted)
                {
                    Manager.Scheduler.Start().Wait();
                }
                Console.WriteLine($"作业调度服务已启动! 当前调度任务 : {Manager. Scheduler.SchedulerName}");
                string userCommand = string.Empty;
                while (userCommand != "exit")
                {
                    if (string.IsNullOrEmpty(userCommand) == false)
                    {
                        Console.WriteLine("     非退出指令,自动忽略...");
                    }
                    userCommand = Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }


        /// <summary>
        /// 启动
        /// </summary>
        public override void Start(JobInfo jobInfo)
        {
            try
            {
                if (jobInfo == null )
                {
                    throw new ArgumentNullException(nameof(jobInfo));
                }

                if (string.IsNullOrWhiteSpace(jobInfo.AssemblyPath))
                {
                    throw new ArgumentNullException("程序集物理路径不能为空");
                }

                if (string.IsNullOrWhiteSpace(jobInfo.ClassType))
                {
                    throw new ArgumentNullException("命名空间不能为空");
                }

                if (string.IsNullOrWhiteSpace(jobInfo.Cron) && jobInfo.Second<=0)
                {
                    throw new ArgumentNullException("Cron 和 Second 不能同时为空");
                }

                if (string.IsNullOrWhiteSpace(jobInfo.JobName))
                {
                    jobInfo.JobName = "test";
                }

                if (string.IsNullOrWhiteSpace(jobInfo.JobGroup))
                {
                    jobInfo.JobGroup = "test";
                }

                if (!Manager.Scheduler.IsStarted)
                {
                    Manager.Scheduler.Start().Wait();
                }
                Console.WriteLine($"作业调度服务已启动! 当前调度任务 : {Manager.Scheduler.SchedulerName}");
                Manager.CreateJob(jobInfo);
                string userCommand = string.Empty;
                while (userCommand != "exit")
                {
                    if (string.IsNullOrEmpty(userCommand) == false)
                    {
                        Console.WriteLine("     非退出指令,自动忽略...");
                    }
                    userCommand = Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}
