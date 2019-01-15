using Go.Job.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Job.IProvider
{
    /// <summary>
    /// Job 数据提供器
    /// </summary>
    public interface IJobInfoProvider
    {
        IList<JobInfo> GetJobInfo();
    }
}
