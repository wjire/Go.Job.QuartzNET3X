using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace TestJob3
{
    public class Job3 : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            string path1 = @"C:\Users\Administrator\Desktop\Job3.txt";
            //string path1 = @"C:\Users\gongwei.LONG\Desktop\Job2.txt";
            using (FileStream fs = new FileStream(path1, FileMode.Append, FileAccess.Write))
            {
                byte[] bytes = Encoding.Default.GetBytes(DateTime.Now + Environment.NewLine);
                fs.Write(bytes, 0, bytes.Length);
            }

            return Task.FromResult(0);
        }
    }
}
