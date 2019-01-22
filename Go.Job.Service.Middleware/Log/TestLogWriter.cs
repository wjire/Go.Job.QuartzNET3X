using System;

namespace Go.Job.Service.Middleware
{
    public class TestLogWriter : ILogWriter
    {
        public void SaveLog(string remark, string content)
        {
            Console.WriteLine(remark + " : " + content);
        }

        public void WriteException(Exception ex, string remark)
        {
            Console.WriteLine(ex);
        }
    }
}
