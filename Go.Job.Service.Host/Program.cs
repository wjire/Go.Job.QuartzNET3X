using System;
using Go.Job.Service.Middleware;

namespace Go.Job.Service.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var logWriter = System.Configuration.ConfigurationManager.AppSettings["LogWriter"];
                if (Convert.ToInt32(logWriter) == 0)
                {
                    MidContainer.ReplaceService(typeof(ILogWriter), new TestLogWriter());
                }
             
                SchedulerManagerFacotry.CreateSchedulerManager().UseJobListener(true).UseTriggerListener(false).Start().Wait();
                string userCommand = string.Empty;
                while (userCommand != "exit")
                {
                    if (string.IsNullOrEmpty(userCommand) == false)
                    {
                        Console.WriteLine("     非退出指令,自动忽略...");
                    }
                    userCommand = Console.ReadLine();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
        }

    }
}
