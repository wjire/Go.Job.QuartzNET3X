using System;
using Go.Job.BaseJob;

namespace TestJob2
{
    public class Job2 : BaseJob
    {
        protected override void Execute()
        {
            Console.WriteLine($"{DateTime.Now} :     Job2 ");
        }
    }
}
