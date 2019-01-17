using Go.Job.Model;
using Go.Job.Service.Job;
using Quartz;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Go.Job.Service
{
    public class JobPoolManager : IDisposable
    {
        internal static ConcurrentDictionary<int, JobRuntimeInfo> JobRuntimePool = new ConcurrentDictionary<int, JobRuntimeInfo>();

        internal static IScheduler Scheduler;

        private static readonly JobPoolManager _jobPoolManager;

        private bool flag = false;

        private static readonly object _lock = new object();
        private JobPoolManager()
        {
        }

        static JobPoolManager()
        {
            _jobPoolManager = new JobPoolManager();
        }

        public static JobPoolManager Instance => _jobPoolManager;


        /// <summary>
        /// 添加job到job池,同时加入到调度任务中.
        /// </summary>
        /// <param name="jobId">job编号</param>
        /// <param name="jobRuntimeInfo"></param>
        /// <returns></returns>
        public bool Add(int jobId, JobRuntimeInfo jobRuntimeInfo)
        {
            lock (_lock)
            {
                try
                {
                    //如果该job实例添加失败,直接返回.
                    if (!JobRuntimePool.TryAdd(jobId, jobRuntimeInfo))
                    {
                        AppDomainLoader.UnLoad(jobRuntimeInfo.AppDomain);
                        return false;
                    }

                    IJobDetail existsedJobDetail = Scheduler.GetJobDetail(new JobKey(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName)).Result;
                    if (existsedJobDetail != null)
                    {
                        //Scheduler.RescheduleJob(new TriggerKey(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName));
                        return false;
                    }
                    IDictionary<string, object> data = new Dictionary<string, object>()
                    {
                        ["JobId"] = jobId
                    };

                    IJobDetail jobDetail = JobBuilder.Create<JobCenter>()
                        .WithIdentity(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName)
                        .SetJobData(new JobDataMap(data))
                        .Build();

                    TriggerBuilder tiggerBuilder = TriggerBuilder.Create().WithIdentity(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName);

                    if (!string.IsNullOrWhiteSpace(jobRuntimeInfo.JobInfo.Cron))
                    {
                        //错过的不管了,剩下的按正常执行
                        tiggerBuilder.WithCronSchedule(jobRuntimeInfo.JobInfo.Cron, c => c.WithMisfireHandlingInstructionDoNothing());
                    }
                    else
                    {
                        tiggerBuilder.WithSimpleSchedule(simple =>
                            {
                                //立刻执行一次,使用总次数
                                simple.WithIntervalInSeconds(jobRuntimeInfo.JobInfo.Second).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires();
                            });
                    }

                    if (jobRuntimeInfo.JobInfo.StartTime > DateTime.Now)
                    {
                        tiggerBuilder.StartAt(jobRuntimeInfo.JobInfo.StartTime);
                    }
                    else
                    {
                        tiggerBuilder.StartNow();
                    }

                    ITrigger trigger = tiggerBuilder.Build();


                    Scheduler.ScheduleJob(jobDetail, trigger).Wait();

                    //TODO:记录日志
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }


        /// <summary>
        /// 获取已添加到job池中的job
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        internal JobRuntimeInfo GetJobFromPool(int jobId)
        {
            if (!JobRuntimePool.ContainsKey(jobId))
            {
                return null;
            }
            lock (_lock)
            {
                if (!JobRuntimePool.ContainsKey(jobId))
                {
                    return null;
                }

                JobRuntimePool.TryGetValue(jobId, out JobRuntimeInfo jobRuntimeInfo);
                return jobRuntimeInfo;
            }
        }


        public virtual void Dispose()
        {
            if (Scheduler != null && !Scheduler.IsShutdown)
            {
                foreach (int jobId in JobRuntimePool.Keys)
                {
                    //将来持久化,可以在这里修改job的状态
                }
                Scheduler.Shutdown();
            }
        }


        /// <summary>
        /// 执行某个job,将job添加到job池.
        /// </summary>
        /// <param name="jobInfo"></param>
        public void Run(JobInfo jobInfo)
        {
            AddJob(jobInfo);
        }


        /// <summary>
        /// 暂停job
        /// </summary>
        /// <param name="jobId"></param>
        public bool Pause(int jobId)
        {
            lock (_lock)
            {
                if (!JobRuntimePool.ContainsKey(jobId))
                {
                    return false;
                }

                JobRuntimeInfo jobRuntimeInfo = GetJobFromPool(jobId);
                if (jobRuntimeInfo.JobInfo.State == 0 || jobRuntimeInfo.JobInfo.State == 1)
                {
                    Scheduler.PauseTrigger(new TriggerKey(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName));
                }
                //TODO:记录日志
                return true;
            }
        }


        /// <summary>
        /// 恢复job
        /// </summary>
        /// <param name="jobId"></param>
        public bool Resume(int jobId)
        {
            lock (_lock)
            {
                if (!JobRuntimePool.ContainsKey(jobId))
                {
                    return false;
                }

                JobRuntimeInfo jobRuntimeInfo = GetJobFromPool(jobId);
                Scheduler.ResumeTrigger(new TriggerKey(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName));
                //TODO:记录日志
                return true;
            }
        }


        /// <summary>
        /// 编辑 job. 该操作 = Remove + Run 
        /// </summary>
        /// <param name="jobInfo"></param>
        public void Update(JobInfo jobInfo)
        {
            Remove(jobInfo.Id);
            Run(jobInfo);
            //TODO:记录日志
        }


        /// <summary>
        /// 从job池中移除某个job,同时卸载该job所在的AppDomain
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public bool Remove(int jobId)
        {
            lock (_lock)
            {
                try
                {
                    if (JobRuntimePool.ContainsKey(jobId))
                    {
                        JobRuntimePool.TryGetValue(jobId, out JobRuntimeInfo jobRuntimeInfo);
                        if (jobRuntimeInfo != null)
                        {
                            TriggerKey triggerKey = new TriggerKey(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName);
                            Scheduler.PauseTrigger(triggerKey);
                            Scheduler.UnscheduleJob(triggerKey);
                            Scheduler.DeleteJob(new JobKey(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName));
                            JobRuntimePool.TryRemove(jobId, out jobRuntimeInfo);
                            AppDomainLoader.UnLoad(jobRuntimeInfo.AppDomain);
                            return true;
                        }
                        //TODO:记录日志
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                }
                return false;
            }
        }


        /// <summary>
        /// 创建新的应用程序域,返回运行时的Job数据
        /// </summary>
        /// <param name="jobInfo"></param>
        internal JobRuntimeInfo CreateJobRuntimeInfo(JobInfo jobInfo)
        {
            lock (_lock)
            {
                JobRuntimeInfo jobRuntimeInfo = null;
                if (JobRuntimePool.ContainsKey(jobInfo.Id))
                {
                    jobRuntimeInfo = GetJobFromPool(jobInfo.Id);
                    return jobRuntimeInfo;
                }
                AppDomain app = Thread.GetDomain();
                BaseJob.BaseJob job = AppDomainLoader.Load(jobInfo.AssemblyPath, jobInfo.ClassTypePath, out app);
                jobRuntimeInfo = new JobRuntimeInfo
                {
                    JobInfo = jobInfo,
                    Job = job,
                    AppDomain = app,
                };
                //TODO:日志记录
                return jobRuntimeInfo;
            }
        }


        /// <summary>
        /// 创建新的应用程序域,并开始执行job
        /// </summary>
        /// <param name="jobInfo"></param>
        internal JobInfo AddJob(JobInfo jobInfo)
        {
            lock (_lock)
            {
                JobRuntimeInfo jobRuntimeInfo = null;
                if (JobRuntimePool.ContainsKey(jobInfo.Id))
                {
                    jobRuntimeInfo = GetJobFromPool(jobInfo.Id);
                    return jobRuntimeInfo.JobInfo;
                }
                AppDomain app = Thread.GetDomain();
                BaseJob.BaseJob job = AppDomainLoader.Load(jobInfo.AssemblyPath, jobInfo.ClassTypePath, out app);
                jobRuntimeInfo = new JobRuntimeInfo
                {
                    JobInfo = jobInfo,
                    Job = job,
                    AppDomain = app,
                };
                bool res = Instance.Add(jobInfo.Id, jobRuntimeInfo);
                if (res == true)
                {
                    jobInfo.State = 1;
                }

                //TODO:日志记录
                return jobInfo;
            }
        }


        public bool RemoveJobRuntimeInfoAndReAdd(JobRuntimeInfo jobRuntimeInfo)
        {
            if (flag)
            {
                return true;
            }

            lock (_lock)
            {
                AppDomain app = Thread.GetDomain();
                jobRuntimeInfo.Job = AppDomainLoader.Load(jobRuntimeInfo.JobInfo.AssemblyPath, jobRuntimeInfo.JobInfo.ClassTypePath, out app);
                jobRuntimeInfo.AppDomain = app;
                JobRuntimePool[jobRuntimeInfo.JobInfo.Id] = jobRuntimeInfo;
                flag = true;
                return flag;
            }
        }
    }
}

