using Go.Job.BaseJob;
using Go.Job.Model;
using System;

namespace Go.Job.Service.Lib
{
    /// <summary>
    /// job在内存中的信息
    /// </summary>
    public class JobRuntimeInfo
    {
        /// <summary>
        /// 应用程序域
        /// </summary>
        public AppDomain AppDomain;

        /// <summary>
        /// 具体的逻辑job
        /// </summary>
        public BaseJob.BaseJob Job { get; set; }

        /// <summary>
        /// jobInfo
        /// </summary>
        public JobInfo JobInfo { get; set; }
    }
}
