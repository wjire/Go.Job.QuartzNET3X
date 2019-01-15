using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Go.Job.Db;
using Go.Job.Model;
using Quartz;

namespace Go.Job.Service
{
    /// <summary>
    /// 扫描job的job
    /// </summary>
    public class ScanJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                IList<JobInfo> jobInfoList = JobInfoDb.GetJobInfoList();
                if (jobInfoList?.Count > 0)
                {
                    foreach (JobInfo jobInfo in jobInfoList)
                    {
                        //只有job池中没有该job,并且该job的状态是 准备中或者执行中,才将该job添加到job池.
                        //如果是执行中,其实理论上是有BUG了,job池和数据库没有同步状态,但是添加到job池不影响操作.
                        if (!JobPoolManager.JobRuntimePool.ContainsKey(jobInfo.Id) && (jobInfo.State == 0 || jobInfo.State == 1))
                        {
                            JobInfo info = JobPoolManager.Instance.AddJobRuntimeInfo(jobInfo);
                            JobInfoDb.UpdateJobState(info);
                        }
                        //如果job池中有该job
                        else if (JobPoolManager.JobRuntimePool.ContainsKey(jobInfo.Id))
                        {
                            //如果状态== 0 和 1 不管它,反正它都在job池了,肯定会执行.

                            if (jobInfo.State == 2)
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
