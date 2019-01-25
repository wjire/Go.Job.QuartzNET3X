using System;
using Go.Job.BaseJob;

namespace TestJob1
{
    public class Job1 : BaseJob
    {
        protected override void Execute()
        {
            Console.WriteLine("揍你");
        }
    }
}
