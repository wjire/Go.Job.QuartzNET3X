using System;
using System.Threading.Tasks;
using Go.Job.Model;
using Go.Job.Service.Lib;
using Quartz;

namespace Go.Job.Service.Core
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
                            Console.WriteLine(ex);
                            Console.WriteLine("重新创建jobRuntimeInfo,替换job池中的jobRuntimeInfo");
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
