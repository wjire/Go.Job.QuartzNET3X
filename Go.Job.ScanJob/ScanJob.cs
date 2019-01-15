using Go.Job.IProvider;
using Go.Job.Model;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Job.Service
{
    /// <summary>
    /// 扫描job的job
    /// </summary>
    public class ScanJob : IJob
    {
        private static readonly IJobInfoProvider defaultJobInfoProvider = (IJobInfoProvider)JobServicesContainer.Instance.GetService(typeof(IJobInfoProvider));

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                System.Collections.Generic.IList<JobInfo> jobInfoList = defaultJobInfoProvider.GetJobInfo();
                foreach (JobInfo jobInfo in jobInfoList)
                {
                    JobPoolManager.Instance.AddJobRuntimeInfo(jobInfo);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return Task.FromResult(0);
        }
    }
}
