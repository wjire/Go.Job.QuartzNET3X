using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Go.Job.Model;
using Go.Job.Service.Job;
using Go.Job.Service.Lib;
using Quartz;

namespace Go.Job.Service
{
    /// <summary>
    /// 调度器管理者
    /// </summary>
    internal class SchedulerManager : IDisposable
    {

        /// <summary>
        /// job池
        /// </summary>
        internal static ConcurrentDictionary<int, JobRuntimeInfo> JobPool = new ConcurrentDictionary<int, JobRuntimeInfo>();

        /// <summary>
        /// 调度器
        /// </summary>
        internal static IScheduler Scheduler;

        /// <summary>
        /// 单例
        /// </summary>
        internal static SchedulerManager Singleton { get; }

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        /// 私有化构造函数
        /// </summary>
        private SchedulerManager()
        {
        }

        static SchedulerManager()
        {
            Singleton = new SchedulerManager();
        }


        /// <summary>
        /// 创建新的应用程序域,返回运行时的Job数据
        /// </summary>
        /// <param name="jobInfo"></param>
        internal JobRuntimeInfo CreateJobRuntimeInfo(JobInfo jobInfo)
        {
            lock (Locker)
            {
                //JobRuntimeInfo jobRuntimeInfo = null;
                //if (JobPool.ContainsKey(jobInfo.Id))
                //{
                //    jobRuntimeInfo = GetJobFromPool(jobInfo.Id);
                //    return jobRuntimeInfo;
                //}
                AppDomain app = Thread.GetDomain();
                BaseJob.BaseJob job = AppDomainLoader.Load(jobInfo.AssemblyPath, jobInfo.ClassType, out app);
                JobRuntimeInfo jobRuntimeInfo = new JobRuntimeInfo
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
        /// 添加job到job池,同时加入到调度任务中.
        /// </summary>
        /// <param name="jobRuntimeInfo"></param>
        /// <returns></returns>
        internal bool Add(JobRuntimeInfo jobRuntimeInfo)
        {
            lock (Locker)
            {
                try
                {
                    //如果该job实例添加失败,卸载该job的appdomain,然后返回.
                    if (!JobPool.TryAdd(jobRuntimeInfo.JobInfo.Id, jobRuntimeInfo))
                    {
                        AppDomainLoader.UnLoad(jobRuntimeInfo.AppDomain);
                        return false;
                    }

                    //如果已经调度任务中已经有该jobDetail,则直接删掉
                    JobKey jobKey = new JobKey(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName);
                    IJobDetail isExistsedJobDetail = Scheduler.GetJobDetail(jobKey).Result;
                    if (isExistsedJobDetail != null)
                    {
                        Scheduler.DeleteJob(jobKey).Wait();
                    }

                    IJobDetail jobDetail = CreateJobDetail(jobRuntimeInfo.JobInfo);
                    ITrigger trigger = CreateTrigger(jobRuntimeInfo.JobInfo);
                    Scheduler.ScheduleJob(jobDetail, trigger).Wait();
                    //TODO:记录日志
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //异常了,直接从job池移除该job,不再考虑移除失败的情况.考虑不到那么多了
                    if (JobPool.TryRemove(jobRuntimeInfo.JobInfo.Id, out JobRuntimeInfo jri))
                    {
                        //成功移除后,再卸载掉应用程序域,失败则不移除,保留.
                        AppDomainLoader.UnLoad(jobRuntimeInfo.AppDomain);
                    }
                    
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
            if (!JobPool.ContainsKey(jobId))
            {
                return null;
            }

            lock (Locker)
            {
                if (!JobPool.ContainsKey(jobId))
                {
                    return null;
                }

                JobPool.TryGetValue(jobId, out JobRuntimeInfo jobRuntimeInfo);
                return jobRuntimeInfo;
            }
        }


        /// <summary>
        /// job池没有该job时,创建 job,并开始调度
        /// TODO:注意,虽然job池没有该job,但是触发器和jobDetail是有的
        /// </summary>
        /// <param name="jobInfo"></param>
        internal bool CreateJob(JobInfo jobInfo)
        {
            if (jobInfo == null)
            {
                return false;
            }
            if (JobPool.ContainsKey(jobInfo.Id))
            {
                return false;
            }
            lock (Locker)
            {
                if (JobPool.ContainsKey(jobInfo.Id))
                {
                    return false;
                }

                JobRuntimeInfo jobRuntimeInfo = CreateJobRuntimeInfo(jobInfo);
                return Add(jobRuntimeInfo);
            }
        }


        public virtual void Dispose()
        {
            if (Scheduler != null && !Scheduler.IsShutdown)
            {
                Scheduler.Shutdown();
            }
        }

        /// <summary>
        /// 暂停job
        /// </summary>
        /// <param name="jobInfo"></param>
        internal bool Pause(JobInfo jobInfo)
        {
            if (!JobPool.ContainsKey(jobInfo.Id))
            {
                return false;
            }

            lock (Locker)
            {
                if (!JobPool.ContainsKey(jobInfo.Id))
                {
                    return false;
                }

                TriggerKey triggerKey = new TriggerKey(jobInfo.JobName, jobInfo.JobName);
                ITrigger isExistsedTriggerKey = Scheduler.GetTrigger(triggerKey).Result;
                if (isExistsedTriggerKey == null) return false;
                Scheduler.PauseTrigger(triggerKey).Wait();
                return true;

                //TODO:记录日志
            }
        }


        /// <summary>
        /// 恢复job
        /// </summary>
        /// <param name="jobInfo"></param>
        internal bool Resume(JobInfo jobInfo)
        {
            //TODO:这里有两种可能
            /*
             * 1.调度服务正常状态时恢复
             * 2.调度服务挂了,重启之后,恢复job.这种情况,job池是没有job的.但是jobDetail和trigger是有的,因为我们采用的是持久化调度器,因此要特殊处理.很重要
             *
             */

            lock (Locker)
            {
                JobRuntimeInfo jobRuntimeInfo = null;
                if (!JobPool.ContainsKey(jobInfo.Id))
                {
                    //如果已经调度任务中没有该jobDetail,则直接返回
                    IJobDetail isExistsedJobDetail = Scheduler.GetJobDetail(new JobKey(jobInfo.JobName, jobInfo.JobName)).Result;
                    if (isExistsedJobDetail == null)
                    {
                        return false;
                    }

                    jobRuntimeInfo = CreateJobRuntimeInfo(jobInfo);
                    //如果该job实例添加失败,卸载appdomain,然后返回.
                    if (!JobPool.TryAdd(jobInfo.Id, jobRuntimeInfo))
                    {
                        AppDomainLoader.UnLoad(jobRuntimeInfo.AppDomain);
                        return false;
                    }
                }
                else
                {
                    jobRuntimeInfo = GetJobFromPool(jobInfo.Id);
                }

                TriggerKey triggerKey = new TriggerKey(jobRuntimeInfo.JobInfo.JobName, jobRuntimeInfo.JobInfo.JobName);
                ITrigger isExistsedTriggerKey = Scheduler.GetTrigger(triggerKey).Result;
                if (isExistsedTriggerKey == null)
                {
                    return false;
                }

                Scheduler.ResumeTrigger(triggerKey).Wait();
                return true;

                //TODO:记录日志
            }
        }


        /// <summary>
        /// 编辑 job. 更新触发器
        /// </summary>
        /// <param name="jobInfo"></param>
        internal bool Update(JobInfo jobInfo)
        {
            lock (Locker)
            {
                TriggerKey triggerKey = new TriggerKey(jobInfo.JobName, jobInfo.JobName);
                ITrigger trigger = CreateTrigger(jobInfo);
                Scheduler.RescheduleJob(triggerKey, trigger);
                return true;
            }
        }


        /// <summary>
        /// 从job池中移除某个job,同时卸载该job所在的AppDomain
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        internal bool Remove(JobInfo jobInfo)
        {
            if (!JobPool.ContainsKey(jobInfo.Id))
            {
                return false;
            }
            lock (Locker)
            {
                if (!JobPool.ContainsKey(jobInfo.Id))
                {
                    return false;
                }
                
                TriggerKey triggerKey = new TriggerKey(jobInfo.JobName, jobInfo.JobName);
                ITrigger isExistsedTriggerKey = Scheduler.GetTrigger(triggerKey).Result;
                if (isExistsedTriggerKey == null)
                {
                    return false;
                }

                Scheduler.PauseTrigger(triggerKey);
                Scheduler.UnscheduleJob(triggerKey);
                Scheduler.DeleteJob(new JobKey(jobInfo.JobName, jobInfo.JobName));
                JobPool.TryRemove(jobInfo.Id, out var jobRuntimeInfo);
                AppDomainLoader.UnLoad(jobRuntimeInfo.AppDomain);
                return true;
                //TODO:记录日志
            }
        }


        /// <summary>
        /// 线程池有job,但是该job的应用程序域已经卸载(一般都是宕机),替换job池中的jobRuntimeInfo,并重新调度该job
        /// </summary>
        /// <param name="jobRuntimeInfo"></param>
        /// <returns></returns>
        internal bool UpdateJobRuntimeInfo(JobRuntimeInfo jobRuntimeInfo)
        {
            //TODO:有BUG,没有地方还原 _flag 的值
            //if (_flag)
            //{
            //    return true;
            //}

            lock (Locker)
            {
                //if (_flag)
                //{
                //    return true;
                //}
                AppDomain app = Thread.GetDomain();
                jobRuntimeInfo.Job = AppDomainLoader.Load(jobRuntimeInfo.JobInfo.AssemblyPath, jobRuntimeInfo.JobInfo.ClassType, out app);
                jobRuntimeInfo.AppDomain = app;
                JobPool[jobRuntimeInfo.JobInfo.Id] = jobRuntimeInfo;
                //_flag = true;
                return true;
            }
        }



        /// <summary>
        /// 更换版本
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        internal bool Upgrade(JobInfo jobInfo)
        {
            lock (Locker)
            {
                Remove(jobInfo);
                JobRuntimeInfo jobRuntimeInfo = CreateJobRuntimeInfo(jobInfo);
                return Add(jobRuntimeInfo);
            }
        }



        /// <summary>
        /// 创建 JobDetail
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        private IJobDetail CreateJobDetail(JobInfo jobInfo)
        {
            IDictionary<string, object> data = new Dictionary<string, object>()
            {
                ["JobId"] = jobInfo.Id,
                ["jobInfo"] = jobInfo
            };

            IJobDetail jobDetail = JobBuilder.Create<JobCenter>()
                .WithIdentity(jobInfo.JobName, jobInfo.JobName)
                .SetJobData(new JobDataMap(data))
                .Build();
            return jobDetail;
        }


        /// <summary>
        /// 创建 Trigger
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
    }
}

