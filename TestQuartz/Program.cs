using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TestQuartz
{
    internal class Program
    {
        private const string HelloJob = "HelloJob";

        private static void Main(string[] args)
        {
            IScheduler sche = new StdSchedulerFactory().GetScheduler().Result;
            sche.Start();

            try
            {

                NewMethod(sche, "job1", "job1Ass", "job1Class");

                NewMethod(sche, "job2", "job2Ass", "job2Class");


                Thread.Sleep(5000);
                var triKey = new TriggerKey("job2", "job2");
                var jobKey = new JobKey("job2", "job2");
                sche.PauseJob(jobKey);
                //sche.PauseTrigger(triKey);

                Update(sche, "job2", "newjob2", "newjob2");

                var count = sche.GetJobKeys(Quartz.Impl.Matchers.GroupMatcher<JobKey>.GroupEquals(HelloJob)).Result.Count;
                Console.WriteLine(count);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Console.ReadKey();
        }

        private static void NewMethod(IScheduler sche, string name, string assemblyPath, string classTypePath)
        {

            IDictionary<string, object> data = new Dictionary<string, object>()
            {
                ["name"] = name,
                ["assemblyPath"] = assemblyPath,
                ["classTypePath"] = classTypePath,
            };

            //创建扫描Job
            var jobDetail = JobBuilder.Create<HelloJob>()
                .SetJobData(new JobDataMap(data))
                  .WithIdentity(name, name)
                  .Build();
            var triggerBuilder = TriggerBuilder.Create()

                .WithIdentity(name, name);
            triggerBuilder.WithSimpleSchedule(s =>
                    s.WithIntervalInSeconds(2).RepeatForever().WithMisfireHandlingInstructionFireNow())
                .StartNow();

            var trigger = triggerBuilder.Build();

            sche.ScheduleJob(jobDetail, trigger);
        }

        private static void Update(IScheduler sche, string name, string assemblyPath, string classTypePath)
        {
            //IDictionary<string, object> data = new Dictionary<string, object>()
            //{
            //    ["name"] = name,
            //    ["assemblyPath"] = assemblyPath,
            //    ["classTypePath"] = classTypePath,
            //};
            //TriggerKey triggerKey = new TriggerKey(name, name);

            //TriggerBuilder tiggerBuilder = TriggerBuilder.Create()
            //    .UsingJobData(new JobDataMap(data))
            //    .WithIdentity(name, name);

            //tiggerBuilder.WithSimpleSchedule(simple =>
            //{
            //    //立刻执行一次,使用总次数
            //    simple.WithIntervalInSeconds(10).RepeatForever()
            //    .WithMisfireHandlingInstructionIgnoreMisfires();
            //});
            //tiggerBuilder.StartNow();
            //ITrigger trigger = tiggerBuilder.Build();

            var jobDetail = sche.GetJobDetail(new JobKey(name, name)).Result;
            var jobKey = new JobKey("job2", "job2");
            var triKey = new TriggerKey(name, name);
            //var trigger = sche.GetTrigger(triKey).Result;

            ////trigger.JobDataMap.Put("assemblyPath", assemblyPath);
            ////trigger.JobDataMap.Put("classTypePath", classTypePath);

            //var builder = trigger.GetTriggerBuilder();
            //builder.UsingJobData("assemblyPath", assemblyPath);
            //builder.UsingJobData("classTypePath", classTypePath);
            //trigger.JobDataMap["assemblyPath"] = assemblyPath;
            //trigger.JobDataMap["classTypePath"] = classTypePath;
            jobDetail.JobDataMap["assemblyPath"] = assemblyPath;
            jobDetail.JobDataMap["classTypePath"] = classTypePath;
            sche.ResumeJob(jobKey);
        }
    }

    [PersistJobDataAfterExecution]
    internal class HelloJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var map = context.JobDetail.JobDataMap;
            //map = context.Trigger.JobDataMap;
            Console.WriteLine($"{DateTime.Now}  {map["name"]} : {map["assemblyPath"]} ,{map["classTypePath"]}");
            return Task.FromResult(0);
        }
    }



}
