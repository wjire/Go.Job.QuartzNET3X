using EastWestWalk.NetFrameWork.Common.Write;
using System;

namespace Go.Job.Service.Middleware
{
    public class DefaultLogWriter : ILogWriter
    {
        public void SaveLog(string remark, string content)
        {
            LogService.SaveLog(remark, content, null);
        }

        public void WriteException(Exception ex, string remark)
        {
            LogService.WriteLog(ex, remark);
        }
    }
}
