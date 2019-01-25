using System;

namespace Go.Job.Service.Middleware
{
    public interface ILogWriter
    {
        void WriteLog(string content, string path = null);


        void WriteException(Exception ex, string remark);
    }
}
