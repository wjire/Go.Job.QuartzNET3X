using EastWestWalk.NetFrameWork.Common.Write;
using System;

namespace Go.Job.Service.Middleware
{
    public class DefaultLogWriter : ILogWriter
    {
        public void WriteLog(string content, string path)
        {
            LogService.WriteLog(content, "JobLogs\\"+path);
        }

        public void WriteException(Exception ex, string remark)
        {
            LogService.WriteLog(ex, remark);
        }
    }
}
