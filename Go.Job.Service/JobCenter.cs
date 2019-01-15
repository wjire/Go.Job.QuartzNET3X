using Quartz;
using System;
using System.Threading.Tasks;
using Job.Service.Model;

namespace Go.Job.Service
{
    public class JobCenter : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var jobId = context.JobDetail.JobDataMap.GetInt("JobId");
                //从作业调度容器里查找，如果找到，则运行
                JobRuntimeInfo jobRuntimeInfo = JobPoolManager.Instance.GetJobFromPool(jobId);
                try
                {
                    jobRuntimeInfo.Job.Run();
                }
                catch (Exception ex)
                {
                    //写日志，job调用失败
                }

            }
            catch (Exception ex)
            {
                //调用的时候失败，写日志，这里的错误属于系统级错误，严重错误
            }

            return Task.FromResult(0);
        }
    }
}
