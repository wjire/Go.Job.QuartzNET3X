using System;
using Go.Job.Model;

namespace Go.Job.Service.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //JobServiceBuilder.BuilderProduce().Start();

            JobServiceBuilder.BuilderTest().Start(new JobInfo
            {
                AssemblyPath = @"E:\gongwei\my\Go.Job\TestJob1\bin\Debug\TestJob1.dll",
                ClassType = "TestJob1.Job1",
                Second = 5,
            });
        }
    }
}
