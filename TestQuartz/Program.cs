using System;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

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

                NewMethod(sche);

                Thread.Sleep(3000);

                NewMethod(sche);

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

        private static void NewMethod(IScheduler sche)
        {
            IJobDetail jobDetail;
            TriggerBuilder triggerBuilder;
            ITrigger trigger;

            //创建扫描Job
            jobDetail = JobBuilder.Create<HelloJob>()
                .WithIdentity(HelloJob, HelloJob)
                .Build();
            triggerBuilder = TriggerBuilder.Create()
.WithIdentity(HelloJob, HelloJob);
            triggerBuilder.WithSimpleSchedule(s =>
                    s.WithIntervalInSeconds(2).RepeatForever().WithMisfireHandlingInstructionFireNow())
                .StartNow();

            trigger = triggerBuilder.Build();
            sche.ScheduleJob(jobDetail, trigger);
        }
    }

    internal class HelloJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"{DateTime.Now} : Hello World");
            return Task.FromResult(0);
        }
    }
}
