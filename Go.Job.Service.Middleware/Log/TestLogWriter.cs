using System;

namespace Go.Job.Service.Middleware
{
    public class TestLogWriter : ILogWriter
    {
        public void WriteLog(string content, string path)
        {
            Console.WriteLine(content);
        }

        public void WriteLog(Exception ex, string remark)
        {
            Console.WriteLine(remark + " : " + ex);
        }
    }
}
