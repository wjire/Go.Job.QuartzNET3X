using System;

namespace Go.Job.Service.Middleware
{
    public class TestLogWriter : ILogWriter
    {
        public void SaveLog(string content, string method = null)
        {
            Console.WriteLine(content);
        }

        public void WriteException(Exception ex, string remark)
        {
            Console.WriteLine(ex);
        }
    }
}
