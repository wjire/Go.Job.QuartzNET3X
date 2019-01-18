using Go.Job.Db;
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

        private bool _flag = false;

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

                    //如果已经调度任务中已经有该jobDetail,则直接返回
                    IJobDetail isExistsedJobDetail = Scheduler.GetJobDetail(new JobKey(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName)).Result;
                    if (isExistsedJobDetail != null)
                    {
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

                    ITrigger trigger = CreateTrigger(jobRuntimeInfo.JobInfo);
                    Scheduler.ScheduleJob(jobDetail, trigger).Wait();
                    //TODO:记录日志
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //异常了,直接从job池移除该job,不再考虑移除失败的情况.考虑不到那么多了
                    JobRuntimePool.TryRemove(jobId, out JobRuntimeInfo jri);
                    return false;
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
        /// 暂停job
        /// </summary>
        /// <param name="jobId"></param>
        public bool Pause(int jobId)
        {
            if (!JobRuntimePool.ContainsKey(jobId))
            {
                return false;
            }

            lock (_lock)
            {
                if (!JobRuntimePool.ContainsKey(jobId))
                {
                    return false;
                }

                JobRuntimeInfo jobRuntimeInfo = GetJobFromPool(jobId);

                TriggerKey triggerKey = new TriggerKey(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName);
                ITrigger isExistsedTriggerKey = Scheduler.GetTrigger(triggerKey).Result;
                if (isExistsedTriggerKey != null)
                {
                    Scheduler.PauseTrigger(triggerKey).Wait();
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
            //TODO:这个逻辑还要梳理
            if (!JobRuntimePool.ContainsKey(jobId))
            {
                JobInfo jobInfo = JobInfoDb.GetJobInfo(jobId);
                //如果已经调度任务中已经有该jobDetail,则直接返回
                var isExistsedJobDetail = Scheduler.GetJobDetail(new JobKey(jobInfo.JobName, jobInfo.JobName)).Result;
                if (isExistsedJobDetail != null)
                {
                    var jobRuntimeInfo = new JobRuntimeInfo();
                    AppDomain app = Thread.GetDomain();
                    jobRuntimeInfo.Job = AppDomainLoader.Load(jobInfo.AssemblyPath, jobInfo.ClassTypePath, out app);
                    jobRuntimeInfo.AppDomain = app;
                    jobRuntimeInfo.JobInfo = jobInfo;
                    //如果该job实例添加失败,直接返回.
                    if (!JobRuntimePool.TryAdd(jobId, jobRuntimeInfo))
                    {
                        AppDomainLoader.UnLoad(jobRuntimeInfo.AppDomain);
                        return false;
                    }
                    TriggerKey triggerKey = new TriggerKey(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName);
                    Scheduler.ResumeTrigger(triggerKey).Wait();
                    return true;
                }
                return false;
            }

            lock (_lock)
            {
                //if (!JobRuntimePool.ContainsKey(jobId))
                //{
                //    return false;
                //}

                JobRuntimeInfo jobRuntimeInfo = GetJobFromPool(jobId);
                TriggerKey triggerKey = new TriggerKey(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName);
                ITrigger isExistsedTriggerKey = Scheduler.GetTrigger(triggerKey).Result;
                if (isExistsedTriggerKey != null)
                {
                    Scheduler.ResumeTrigger(triggerKey).Wait();
                    return true;
                }
                else
                {
                    return false;
                }

                //TODO:记录日志
            }
        }


        /// <summary>
        /// 编辑 job. 更新触发器
        /// </summary>
        /// <param name="jobInfo"></param>
        public bool Update(JobInfo jobInfo)
        {
            lock (_lock)
            {
                TriggerKey triggerKey = new TriggerKey(jobInfo.JobName, jobInfo.JobName);
                ITrigger trigger = CreateTrigger(jobInfo);
                Scheduler.RescheduleJob(triggerKey, trigger);
                return true;
            }
        }

        /// <summary>
        /// 创建触发器
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        private ITrigger CreateTrigger(JobInfo jobInfo)
        {
            TriggerBuilder tiggerBuilder = TriggerBuilder.Create().WithIdentity(jobInfo.JobName, jobInfo.JobName);

            if (!string.IsNullOrWhiteSpace(jobInfo.Cron))
            {
                tiggerBuilder.WithCronSchedule(jobInfo.Cron, c => c.WithMisfireHandlingInstructionFireAndProceed());
            }
            else
            {
                tiggerBuilder.WithSimpleSchedule(simple =>
                {
                    simple.WithIntervalInSeconds(jobInfo.Second).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires();
                });
            }

            if (jobInfo.StartTime > DateTime.Now)
            {
                tiggerBuilder.StartAt(jobInfo.StartTime);
            }
            else
            {
                tiggerBuilder.StartNow();
            }

            ITrigger trigger = tiggerBuilder.Build();
            return trigger;
        }


        /// <summary>
        /// 从job池中移除某个job,同时卸载该job所在的AppDomain
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public bool Remove(int jobId)
        {
            if (!JobRuntimePool.ContainsKey(jobId))
            {
                return false;
            }
            lock (_lock)
            {
                if (!JobRuntimePool.ContainsKey(jobId))
                {
                    return false;
                }

                JobRuntimePool.TryGetValue(jobId, out JobRuntimeInfo jobRuntimeInfo);
                if (jobRuntimeInfo == null)
                {
                    return false;
                }

                TriggerKey triggerKey = new TriggerKey(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName);
                ITrigger isExistsedTriggerKey = Scheduler.GetTrigger(triggerKey).Result;
                if (isExistsedTriggerKey == null)
                {
                    return false;
                }

                Scheduler.PauseTrigger(triggerKey);
                Scheduler.UnscheduleJob(triggerKey);
                Scheduler.DeleteJob(new JobKey(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName));
                JobRuntimePool.TryRemove(jobId, out jobRuntimeInfo);
                AppDomainLoader.UnLoad(jobRuntimeInfo.AppDomain);
                return true;
                //TODO:记录日志
            }
        }

        /// <summary>
        /// job池没有该job时,创建 job,并开始调度
        /// TODO:注意,虽然job池没有该job,但是触发器和jobDetail是有的
        /// </summary>
        /// <param name="jobId"></param>
        internal bool CreateJob(int jobId)
        {
            if (JobRuntimePool.ContainsKey(jobId))
            {
                return true;
            }
            lock (_lock)
            {
                if (JobRuntimePool.ContainsKey(jobId))
                {
                    return true;
                }
                JobInfo jobInfo = JobInfoDb.GetJobInfo(jobId);
                if (jobInfo == null || jobInfo.Id == 0)
                {
                    throw new Exception($"获取JobInfo失败, id = {jobId}");
                }

                JobRuntimeInfo jobRuntimeInfo = null;
                AppDomain app = Thread.GetDomain();
                BaseJob.BaseJob job = AppDomainLoader.Load(jobInfo.AssemblyPath, jobInfo.ClassTypePath, out app);
                jobRuntimeInfo = new JobRuntimeInfo
                {
                    JobInfo = jobInfo,
                    Job = job,
                    AppDomain = app,
                };
                //TODO:日志记录
                return Add(jobId, jobRuntimeInfo);
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


        /// <summary>
        /// 线程池有job,但是该job的应用程序域已经卸载(一般都是宕机),替换job池中的jobRuntimeInfo,并重新调度该job
        /// </summary>
        /// <param name="jobRuntimeInfo"></param>
        /// <returns></returns>
        public bool UpdateJobRuntimeInfo(JobRuntimeInfo jobRuntimeInfo)
        {
            //TODO:有BUG,没有地方还原 _flag 的值
            if (_flag)
            {
                return true;
            }

            lock (_lock)
            {
                if (_flag)
                {
                    return true;
                }
                AppDomain app = Thread.GetDomain();
                jobRuntimeInfo.Job = AppDomainLoader.Load(jobRuntimeInfo.JobInfo.AssemblyPath, jobRuntimeInfo.JobInfo.ClassTypePath, out app);
                jobRuntimeInfo.AppDomain = app;
                JobRuntimePool[jobRuntimeInfo.JobInfo.Id] = jobRuntimeInfo;
                _flag = true;
                return _flag;
            }
        }
    }
}

