using System;
using System.Threading.Tasks;
using Go.Job.Model;
using Quartz;

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
                    jobRuntimeInfo?.Job.Run();
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
