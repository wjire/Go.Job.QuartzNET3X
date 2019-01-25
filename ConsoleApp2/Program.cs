using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Go.Job.Service;
using Go.Job.Service.Middleware;
using ServiceContainer = System.ComponentModel.Design.ServiceContainer;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //MidContainer.ReplaceService(typeof(ILogWriter), new TestLogWriter());
                //SchedulerManagerFacotry.CreateSchedulerManager().Start().Wait();
                //string userCommand = string.Empty;
                //while (userCommand != "exit")
                //{
                //    if (string.IsNullOrEmpty(userCommand) == false)
                //    {
                //        Console.WriteLine("     非退出指令,自动忽略...");
                //    }
                //    userCommand = Console.ReadLine();
                //}


                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        throw new Exception("测试异步异常");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
        }
    }
}
