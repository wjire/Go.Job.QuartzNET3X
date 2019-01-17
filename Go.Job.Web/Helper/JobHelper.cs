using System;
using System.Collections.Specialized;
using Go.Job.Db;
using Go.Job.Model;
using Quartz;
using Quartz.Simpl;

namespace Go.Job.Web.Helper
{
    internal static class JobHelper
    {
        public static readonly IRemotableQuartzScheduler Scheduler;

        static JobHelper()
        {
            if (Scheduler == null)
            {
                NameValueCollection properties = new NameValueCollection();
                properties["quartz.scheduler.instanceName"] = "调度作业监控系统";
                properties["quartz.scheduler.proxy"] = "true";
                properties["quartz.scheduler.proxy.address"] = "tcp://127.0.0.1:555/QuartzScheduler";
                RemotingSchedulerProxyFactory proxyFactory = new RemotingSchedulerProxyFactory
                {
                    Address = "tcp://127.0.0.1:555/QuartzScheduler"
                };
                Scheduler = proxyFactory.GetProxy();
                if (Scheduler.IsShutdown)
                {
                    Scheduler.Start();
                }
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool Run(int id, string name)
        {
            try
            {
                if (id == 0)
                {
                    return false;
                }

                string path = @"http://localhost:25250/api/job/add?id=" + id;
                string code = HttpClientHelper.GetString(path);
                if (Convert.ToInt32(code) == 200)
                {
                    int dbRes = JobInfoDb.UpdateJobState(new JobInfo { State = 1, Id = id });
                    if (dbRes > 0)
                    {
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return false;

        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Pause(int id)
        {
            try
            {
                if (id == 0)
                {
                    return false;
                }

                string path = @"http://localhost:25250/api/job/pause?id=" + id;
                string code = HttpClientHelper.GetString(path);
                if (Convert.ToInt32(code) == 200)
                {
                    int dbRes = JobInfoDb.UpdateJobState(new JobInfo { State = 2, Id = id });
                    if (dbRes > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
            }

            return false;
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Resume(int id)
        {
            try
            {
                if (id == 0)
                {
                    return false;
                }

                string path = @"http://localhost:25250/api/job/resume?id=" + id;
                string code = HttpClientHelper.GetString(path);
                if (Convert.ToInt32(code) == 200)
                {
                    int dbRes = JobInfoDb.UpdateJobState(new JobInfo { State = 1, Id = id });
                    if (dbRes > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
            }

            return false;
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Remove(int id)
        {
            try
            {
                if (id == 0)
                {
                    return false;
                }

                string path = @"http://localhost:25250/api/job/remove?id=" + id;
                string code = HttpClientHelper.GetString(path);
                if (Convert.ToInt32(code) == 200)
                {
                    int dbRes = JobInfoDb.UpdateJobState(new JobInfo { State = 3, Id = id });
                    if (dbRes > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
            }

            return false;
        }



        public static bool Update(JobInfo jobInfo)
        {
            bool updateRes = false;
            try
            {
                int dbRes = JobInfoDb.UpdateJobInfo(jobInfo);
                if (dbRes == 0)
                {
                    //TODO:数据库修改失败,记录日志
                }
                else
                {
                    TriggerKey triggerKey = new TriggerKey(jobInfo.JobName, jobInfo.JobName);

                    TriggerBuilder tiggerBuilder = TriggerBuilder.Create().WithIdentity(jobInfo.JobName, jobInfo.JobName);


                    tiggerBuilder.WithSimpleSchedule(simple =>
                    {
                        //立刻执行一次,使用总次数
                        simple.WithIntervalInSeconds(jobInfo.Second).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires();
                    });

                    tiggerBuilder.StartNow();

                    ITrigger trigger = tiggerBuilder.Build();

                    Scheduler.RescheduleJob(triggerKey, trigger);
                    updateRes = true;
                }
            }
            catch (Exception e)
            {

            }
            return updateRes;
        }
    }
}