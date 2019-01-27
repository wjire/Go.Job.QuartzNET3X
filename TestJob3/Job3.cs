using System;
using Go.Job.BaseJob;

namespace TestJob3
{
    public class Job3 : BaseJob
    {
        protected override void Execute()
        {
            //Console.WriteLine($"{DateTime.Now} : 我归 refuge 控制台程序(调度器)管 ");
            Console.WriteLine($"{DateTime.Now} :         Job3 ");
        }
    }
}
