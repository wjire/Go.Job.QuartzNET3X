using System;

namespace Go.Job.Service.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SchedulerManagerFactory.CreateSchedulerManager().Start();
        }
    }
}
