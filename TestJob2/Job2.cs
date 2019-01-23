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
            Console.WriteLine($"{DateTime.Now} : Job2 Run......");
        }
    }
}
