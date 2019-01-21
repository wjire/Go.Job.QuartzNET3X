using System;
using EastWestWalk.NetFrameWork.Common.Write;

namespace Go.Job.Service.Middleware
{
    public class DefaultLogWriter : ILogWriter
    {
        public void SaveLog(string content, string method = null)
        {
            LogService.WriteLog(content);
        }

        public void WriteException(Exception ex, string remark)
        {
            LogService.WriteLog(ex, remark);
        }
    }
}
