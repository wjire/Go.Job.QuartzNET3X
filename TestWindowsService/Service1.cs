using Go.Job.Service.Middleware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Go.Job.Service;

namespace TestWindowsService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //var logWriter = System.Configuration.ConfigurationManager.AppSettings["LogWriter"];
                //if (Convert.ToInt32(logWriter) == 0)
                //{
                    MidContainer.ReplaceService(typeof(ILogWriter), new TestLogWriter());
                //}

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

        protected override void OnStop()
        {
        }
    }
}
