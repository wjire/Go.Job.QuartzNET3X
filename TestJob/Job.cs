using System;
using System.IO;
using System.Text;
using System.Threading;
using Go.Job.BaseJob;

namespace TestJob
{
    public class Job : BaseJob
    {
        protected override void Execute()
        {
            //throw new Exception("测试抛异常");
            //Console.WriteLine($"{DateTime.Now} : Job2 Run......");


            //string path1 = @"C:\Users\Administrator\Desktop\Job.txt";
            string path1 = @"C:\Users\gongwei.LONG\Desktop\testwindowservice.txt";
            using (FileStream fs = new FileStream(path1, FileMode.Append, FileAccess.Write))
            {
                byte[] bytes = Encoding.Default.GetBytes(DateTime.Now + Environment.NewLine);
                fs.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
