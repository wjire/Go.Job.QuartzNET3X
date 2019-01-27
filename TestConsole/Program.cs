using System;
using Quartz;
using Quartz.Impl;
using TestJob1;

namespace TestConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string assemblyPath = @"H:\0开发项目\Go.Job.QuartzNET\TestJob1\bin\1\TestJob1.dll";
            string classType = "TestJob1.Job1";
            AppDomain appDomain = null;
            Type type = GetJobDetailType(assemblyPath, classType, out appDomain);

            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            scheduler.Start();

            string jobName = "job1";
            string triName = "tri1";
            Start(type, scheduler, jobName, triName);

            //Thread.Sleep(10000);//在这10秒钟内,更新 TestJob1.dll 
            //scheduler.DeleteJob(new JobKey(jobName, jobName));//从调度器中删掉job1
            //AppDomain.Unload(appDomain);//卸载job1的应用程序域
            //appDomain = null;

            //assemblyPath = @"H:\0开发项目\Go.Job.QuartzNET\TestJob1\bin\1\TestJob1.dll";
            //classType = "TestJob1.Job1";
            //type = GetJobDetailType(assemblyPath, classType, out appDomain);
            //Start(type, scheduler, jobName, triName);

            Console.ReadKey();
        }

        private static void Start(Type type, IScheduler scheduler, string jobName, string triName)
        {
            IJobDetail jobDetail = JobBuilder.Create(type).WithIdentity(jobName, jobName).Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity(triName, triName)
                .WithSimpleSchedule(s => s.WithIntervalInSeconds(3)
                    .RepeatForever()).StartNow()
                .Build();

            scheduler.ScheduleJob(jobDetail, trigger);
        }

        private static Type GetJobDetailType(string assemblyPath, string classType, out AppDomain appDomain)
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ShadowCopyFiles = "true";
            setup.ApplicationBase = System.IO.Path.GetDirectoryName(assemblyPath);
            string appDomainName = System.IO.Path.GetFileName(assemblyPath);
            appDomain = AppDomain.CreateDomain(appDomainName, null, setup);
            object job = (Job1)appDomain.CreateInstanceFromAndUnwrap(assemblyPath, classType);
            Type type = job.GetType();
            return type;
        }
    }
}
