using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using System.Timers;
using Go.Job.Service;
using Go.Job.Service.Middleware;

namespace TestWindowService
{
    public class TownCrier
    {
        readonly Timer _timer;
        public TownCrier()
        {
            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) => Console.WriteLine("It is {0} and all is well", DateTime.Now);
        }
        public void Start() { _timer.Start(); }
        public void Stop() { _timer.Stop(); }
    }

    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(x =>                                 //1
            {
                //x.Service<TownCrier>(s =>                        //2
                //{
                //    s.ConstructUsing(name => new TownCrier());     //3
                //    s.WhenStarted(tc => tc.Start());              //4
                //    s.WhenStopped(tc => tc.Stop());               //5
                //});

                x.Service<TestQuartz>(t =>
                {
                    t.ConstructUsing(name => new TestQuartz());
                    t.WhenStarted(tw => tw.Start());
                    t.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();                            //6

                x.SetDescription("test Topshelf Host");        //7
                x.SetDisplayName("test");                       //8
                x.SetServiceName("test");                       //9
            });                                                  //10
        }
    }


    class TestQuartz
    {
        public void Start()
        {
             try
            {
                var logWriter = System.Configuration.ConfigurationManager.AppSettings["LogWriter"];
                if (Convert.ToInt32(logWriter) == 0)
                {
                    MidContainer.ReplaceService(typeof(ILogWriter), new TestLogWriter());
                }

                SchedulerManagerFacotry.CreateSchedulerManager().UseJobListener(true).UseTriggerListener(false).Start().Wait();
                //string userCommand = string.Empty;
                //while (userCommand != "exit")
                //{
                //    if (string.IsNullOrEmpty(userCommand) == false)
                //    {
                //        Console.WriteLine("     非退出指令,自动忽略...");
                //    }
                //    userCommand = Console.ReadLine();
                //}

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            //Console.ReadKey();
        }

        public void Stop()
        {
            
        }
    }
}
