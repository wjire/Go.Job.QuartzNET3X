using System;
using System.Threading.Tasks;
using Go.Job.Model;
using Go.Job.Service.Middleware;
using Quartz;

namespace Go.Job.Service.Logic
{
    /// <summary>
    /// job中心,所有的job都是从这里开始执行
    /// </summary>
    [DisallowConcurrentExecution]
    public class JobCenter : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                JobInfo jobInfo = context.JobDetail.JobDataMap.Get("jobInfo") as JobInfo ?? new JobInfo();
                //从作业调度容器里查找，如果找到，则运行
                JobRuntimeInfo jobRuntimeInfo = SchedulerManager.Singleton.GetJobFromPool(jobInfo.Id);

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
                            MidInUsed.LogWriter.WriteException(ex, "appdomain 被卸载,准备重新加载");
                            SchedulerManager.Singleton.ReplaceJobRuntimeInfo(jobRuntimeInfo);
                        }
                    }
                    //如果job池没有该job
                    //TODO:注意,虽然job池没有该job,但是触发器和jobDetail是有的,不然也不会进到这个job的 execute 方法
                    else
                    {
                        SchedulerManager.Singleton.CreateJob(jobInfo);
                    }

                }
                catch (Exception ex)
                {
                    //写日志，job调用失败
                    MidInUsed.LogWriter.WriteException(ex, nameof(Execute));
                }

            }
            catch (Exception ex)
            {
                //TODO:调用的时候失败属于系统级错误,非常重要,写日志!
                MidInUsed.LogWriter.WriteException(ex, nameof(Execute));
            }

            return Task.FromResult(0);
        }
    }
}
