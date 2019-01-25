using System;
using EastWestWalk.NetFrameWork.Common.Write;

namespace Go.Job.Service.Middleware
{
    public class DefaultLogWriter : ILogWriter
    {
        public void WriteLog(string content, string path = null)
        {
            LogService.WriteLog(content, "Logs\\" + path ?? string.Empty);
        }

        public void WriteException(Exception ex, string remark)
        {
            LogService.WriteLog(ex, remark);
        }
    }
}
