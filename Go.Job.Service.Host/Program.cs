using System;
using Go.Job.Model;

namespace Go.Job.Service.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //SchedulerManagerFactory.CreateSchedulerManager().Start();


            //SchedulerManagerFactory.CreateTestSchedulerManager().Test.CreateJob(new JobInfo
            //{
            //    AssemblyPath = @"E:\gongwei\my\Go.Job\TestJob1\bin\Debug\TestJob1.dll",
            //    ClassType = "TestJob1.Job1",
            //    JobName = "test",
            //    JobGroup = "test",
            //    Second = 5,
            //});

            JobServiceBuilder.BuilderTest().Start(new JobInfo
            {
                AssemblyPath = @"E:\gongwei\my\Go.Job\TestJob1\bin\Debug\TestJob1.dll",
                ClassType = "TestJob1.Job1",
                Second = 5,
            });
        }
    }
}
