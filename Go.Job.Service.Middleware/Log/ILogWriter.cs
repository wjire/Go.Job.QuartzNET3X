using System;

namespace Go.Job.Service.Middleware
{
    public interface ILogWriter
    {
        void WriteLog(string content, string path);

        void WriteException(Exception ex, string remark);
    }
}
