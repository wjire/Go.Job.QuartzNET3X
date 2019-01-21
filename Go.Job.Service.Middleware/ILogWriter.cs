using System;

namespace Go.Job.Service.Middleware
{
    public interface ILogWriter
    {
        void SaveLog(string content, string method = null);


        void WriteException(Exception ex, string method);
    }
}
