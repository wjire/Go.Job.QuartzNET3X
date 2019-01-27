using System;
using Go.Job.BaseJob;

namespace TestJob4
{
    public class Job4 : BaseJob
    {
        protected override void Execute()
        {
            //Console.WriteLine($"{DateTime.Now} : 我是 Job4 /*,我归 refuge 控制台程序(调度器)管*/ ");
            Console.WriteLine($"{DateTime.Now} :              Job4  ");
        }
    }
}
