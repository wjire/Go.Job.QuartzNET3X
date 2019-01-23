using Go.Job.BaseJob;
using System;
using System.IO;
using System.Text;

namespace TestJob2
{
    public class Job2 : BaseJob
    {
        protected override void Execute()
        {
            throw new Exception("测试抛异常");
            Console.WriteLine($"{DateTime.Now} : Job2 Run......");
        }
    }
}
