using Go.Job.BaseJob;
using Go.Job.Model;
using System;

namespace Go.Job.Service.Lib
{
    public class JobRuntimeInfo
    {
        public AppDomain AppDomain;

        public BaseJob.BaseJob Job { get; set; }

        public JobInfo JobInfo { get; set; }
    }
}
