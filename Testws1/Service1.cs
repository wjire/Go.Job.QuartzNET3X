using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Testws1
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Go.Job.Service.Host.Program.Main(args);
            //string path = @"C:\Users\Administrator\Desktop\testws1.txt";
            ////string path = @"C:\Users\gongwei.LONG\Desktop\Job2.txt";
            //using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            //{
            //    byte[] bytes = Encoding.Default.GetBytes(DateTime.Now + Environment.NewLine);
            //    fs.Write(bytes, 0, bytes.Length);
            //}
        }

        protected override void OnStop()
        {
        }
    }
}
