using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Go.Job.Model;
using Go.Job.Service;

namespace Test3
{
    class Program
    {
        static void Main(string[] args)
        {
            JobServiceBuilder.BuilderTest().Start(new JobInfo
            {
                AssemblyPath = @"E:\gongwei\my\Go.Job\TestJob2\bin\Debug\TestJob2.dll",
                ClassType = "TestJob2.Job2",
                Second = 5,
            });
        }
    }
}
