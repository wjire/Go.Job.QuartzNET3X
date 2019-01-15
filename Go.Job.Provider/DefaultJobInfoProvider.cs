using Go.Job.Db;
using Go.Job.IProvider;
using Go.Job.Model;
using System.Collections.Generic;

namespace Go.Job.Provider
{
    /// <summary>
    /// 系统默认的 Job 数据提供器
    /// </summary>
    public class DefaultJobInfoProvider : IJobInfoProvider
    {
        public IList<JobInfo> GetJobInfo()
        {
            return JobInfoDb.GetJobInfoList();
        }
    }
}
