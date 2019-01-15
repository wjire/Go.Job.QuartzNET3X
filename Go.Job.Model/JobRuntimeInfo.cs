using Go.Job.BaseJob;
using Go.Job.Model;
using System;

namespace Job.Service.Model
{
    public class JobRuntimeInfo
    {
        public AppDomain AppDomain;

        public BaseJob Job { get; set; }

        public JobInfo JobInfo { get; set; }
    }
}
