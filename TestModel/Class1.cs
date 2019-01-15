using Go.Job.BaseJob;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace TestModel
{
    public class Job : BaseJob
    {
        public override void Run()
        {
            //string path1 = @"C:\Users\Administrator\Desktop\Job.txt";
            string path1 = @"C:\Users\gongwei.LONG\Desktop\Job.txt";
            using (FileStream fs = new FileStream(path1, FileMode.Append, FileAccess.Write))
            {
                byte[] bytes = Encoding.Default.GetBytes(DateTime.Now + Environment.NewLine);
                fs.Write(bytes, 0, bytes.Length);
            }

            //string name = Thread.GetDomain().FriendlyName;
            //Tools.FileHelper.WriteString(name);
        }
    }
}
