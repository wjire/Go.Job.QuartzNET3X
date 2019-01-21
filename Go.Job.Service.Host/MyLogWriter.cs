using EastWestWalk.NetFrameWork.Common.Write;
using Go.Job.Model;
using System;

namespace Go.Job.Service.Host
{
    public class MyLogWriter : ILogWriter
    {
        public void WriteLogBeforeStart(JobLog log)
        {
            LogService.SaveLog("WriteLogBeforeStart", $"{DateTime.Now} : {log.JobInfo.JobName} 即将开始执行!", null);
        }

        public void WriteLogAfterEnd(JobLog log)
        {
            LogService.SaveLog("WriteLogBeforeStart", $"{DateTime.Now} : {log.JobInfo.JobName} 执行完毕!", null);
        }

        public void WriteException(Exception ex, string method)
        {
            LogService.WriteLog(ex, $"{DateTime.Now} : {method} 方法发生异常");
        }
    }
}
