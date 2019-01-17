using Go.Job.Db;
using Go.Job.Model;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Go.Job.Service.Job
{
    /// <summary>
    /// 扫描job的job
    /// </summary>
    [PersistJobDataAfterExecution]
    public class ScanJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                IList<JobInfo> jobInfoList = JobInfoDb.GetJobInfoList();//TODO:再封装
                if (jobInfoList?.Count > 0)
                {
                    //TODO:所有JOB的状态以数据库为准!
                    foreach (JobInfo jobInfo in jobInfoList)
                    {
                        //只有job池中没有该job,并且该job的状态是 准备中,才将该job添加到job池.
                        if (!JobPoolManager.JobRuntimePool.ContainsKey(jobInfo.Id) && jobInfo.State == 1)
                        {
                            JobRuntimeInfo jobRuntimeJob = JobPoolManager.Instance.CreateJobRuntimeInfo(jobInfo);
                            JobPoolManager.Instance.Add(jobInfo.Id, jobRuntimeJob);
                        }
                        //如果job池中有该job
                        else if (JobPoolManager.JobRuntimePool.ContainsKey(jobInfo.Id))
                        {
                            //如果状态== 0 和 1 
                            if (jobInfo.State == 0 || jobInfo.State == 1)
                            {
                                JobPoolManager.JobRuntimePool.TryRemove(jobInfo.Id, out JobRuntimeInfo jobRuntimeInfo);
                                if (jobRuntimeInfo != null)
                                {
                                    try
                                    {
                                        AppDomainLoader.UnLoad(jobRuntimeInfo.AppDomain);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e.Message);
                                    }
                                    JobInfo info = JobPoolManager.Instance.AddJob(jobInfo);
                                    JobInfoDb.UpdateJobState(info);
                                }
                            }
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
                string path1 = @"C:\Users\gongwei.LONG\Desktop\exception.txt";
                using (FileStream fs = new FileStream(path1, FileMode.Append, FileAccess.Write))
                {
                    byte[] bytes = Encoding.Default.GetBytes(e + Environment.NewLine);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            return Task.FromResult(0);
        }
    }
}
