using System;
using System.Threading;
using Go.Job.BaseJob;

namespace TestJob
{
    public class Job : BaseJob
    {
        protected override void Execute()
        {
            //throw new Exception("测试抛异常");
            Console.WriteLine($"{DateTime.Now} : Job Run......");
        }
    }
}
