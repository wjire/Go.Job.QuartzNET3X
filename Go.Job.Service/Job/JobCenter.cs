using Go.Job.Db;
using Go.Job.Model;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Go.Job.Service.Job
{
    public class JobCenter : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                int jobId = context.JobDetail.JobDataMap.GetInt("JobId");
                //从作业调度容器里查找，如果找到，则运行
                JobRuntimeInfo jobRuntimeInfo = JobPoolManager.Instance.GetJobFromPool(jobId);

                try
                {
                    if (jobRuntimeInfo != null)
                    {
                        try
                        {
                            jobRuntimeInfo.Job.Run();
                        }
                        catch (AppDomainUnloadedException ex)
                        {
                            Console.WriteLine("AppDomain 已经卸载");
                            Console.WriteLine("从job池删除该job");
                            JobPoolManager.Instance.RemoveJobRuntimeInfoAndReAdd(jobRuntimeInfo);
                        }
                    }
                    //如果没有 ,创建
                    else
                    {
                        var jobInfo = JobInfoDb.GetJobInfo(jobId);
                        if (jobInfo == null || jobInfo.Id == 0)
                        {
                            throw new Exception($"获取JobInfo失败, id = {jobId}");
                        }

                        jobRuntimeInfo = JobPoolManager.Instance.CreateJobRuntimeInfo(jobInfo);
                        JobPoolManager.Instance.Add(jobId, jobRuntimeInfo);
                    }

                }
                catch (Exception ex)
                {
                    //写日志，job调用失败
                    Console.WriteLine(ex);
                }

            }
            catch (Exception ex)
            {
                //TODO:调用的时候失败属于系统级错误,非常重要,写日志!
                Console.WriteLine(ex);
            }

            return Task.FromResult(0);
        }
    }
}
