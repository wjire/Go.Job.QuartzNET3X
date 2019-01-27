using System;
using Go.Job.Model;

namespace Go.Job.Service.Logic
{
    /// <summary>
    /// Job运行时信息
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
