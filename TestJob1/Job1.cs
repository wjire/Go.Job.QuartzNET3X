using System;
using Go.Job.BaseJob;

namespace TestJob1
{
    //public class Job1 : BaseJob
    //{
    //    protected override void Execute()
    //    {
    //        Console.WriteLine($"{DateTime.Now} : Job1");
    //        Assembly assembly = Assembly.LoadFile("程序集物理路径");
    //        Type type = assembly.GetType("类型完全限定名");
    //    }
    //}





    //public class Job1 : BaseJob.BaseJob
    //{
    //    protected override void Run()
    //    {
    //        Console.WriteLine("版本1");
    //    }
    //}


    public class Job1 : BaseJob
    {
        protected override void Execute()
        {
            //Console.WriteLine($"{DateTime.Now} : 我是 Job1 , 我归 wechat 控制台程序(调度器)管");
            Console.WriteLine($"{DateTime.Now} :  Job1 ");
        }
    }
}
