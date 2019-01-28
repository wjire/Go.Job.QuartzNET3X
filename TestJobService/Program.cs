using Go.Job.Model;
using Go.Job.Service;

namespace TestJobService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            JobServiceBuilder.BuilderTest().Start(new JobInfo
            {
                AssemblyPath = @"E:\gongwei\my\Go.Job\TestJob1\bin\Debug\TestJob1.dll",
                ClassType = "TestJob1.Job1",
                Second = 5,
            });
        }
    }
}
