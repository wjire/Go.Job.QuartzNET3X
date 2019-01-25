using System;
using Go.Job.Service.Middleware;

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
