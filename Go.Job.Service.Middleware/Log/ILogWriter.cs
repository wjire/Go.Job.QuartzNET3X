using System;

namespace Go.Job.Service.Middleware
{
    public interface ILogWriter
    {
        void SaveLog(string remark, string content);

        void WriteException(Exception ex, string remark);
    }
}
