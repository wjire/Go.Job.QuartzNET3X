using System;
using System.Threading.Tasks;
using Go.Job.Model;
using Quartz;

namespace Go.Job.Service.Job
{
    [DisallowConcurrentExecution]
    public class JobCenter : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                JobInfo jobInfo = context.JobDetail.JobDataMap.Get("jobInfo") as JobInfo ?? new JobInfo();
                //从作业调度容器里查找，如果找到，则运行
                JobRuntimeInfo jobRuntimeInfo = JobPoolManager.Instance.GetJobFromPool(jobInfo.Id);

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
                            Console.WriteLine(ex);
                            Console.WriteLine("重新创建jobRuntimeInfo,替换job池中的jobRuntimeInfo");
                            JobPoolManager.Instance.UpdateJobRuntimeInfo(jobRuntimeInfo);
                        }
                    }
                    //如果job池没有该job
                    //TODO:注意,虽然job池没有该job,但是触发器和jobDetail是有的,不然也不会进到这个job的 execute 方法
                    else
                    {
                        JobPoolManager.Instance.CreateJob(jobInfo);
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
