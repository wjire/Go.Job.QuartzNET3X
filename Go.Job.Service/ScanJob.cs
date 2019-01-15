using Go.Job.Db;
using Go.Job.IProvider;
using Go.Job.Model;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Go.Job.Service
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
                IList<JobInfo> jobInfoList = defaultJobInfoProvider.GetJobInfo();
                if (jobInfoList?.Count > 0)
                {
                    foreach (JobInfo jobInfo in jobInfoList)
                    {
                        //只有job池中没有该job,并且该job的状态是 准备中,才将该job添加到job池
                        if (!JobPoolManager.JobRuntimePool.ContainsKey(jobInfo.Id) && jobInfo.State == 0)
                        {
                            var info = JobPoolManager.Instance.AddJobRuntimeInfo(jobInfo);
                            JobInfoDb.UpdateJobState(info);
                        }
                        //如果job池中有该job,处理不在 准备中 的job
                        else if (JobPoolManager.JobRuntimePool.ContainsKey(jobInfo.Id))
                        {
                            if (jobInfo.State == 1)
                            {
                                JobPoolManager.Instance.Resume(jobInfo.Id);
                            }
                            else if (jobInfo.State == 2)
                            {
                                JobPoolManager.Instance.Pause(jobInfo.Id);
                            }
                            else if (jobInfo.State == 3)
                            {
                                JobPoolManager.Instance.Remove(jobInfo.Id);
                            }
                        }
                    }
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
