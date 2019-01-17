using Go.Job.Db;
using Go.Job.Model;
using Quartz;
using Quartz.Simpl;
using System;
using System.Collections.Specialized;

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

                if (string.IsNullOrWhiteSpace(name))
                {
                    return false;
                }

                var job = Scheduler.GetJobDetail(new JobKey(name, name));
                if (job != null)
                {
                    int dbRes = JobInfoDb.UpdateJobState(new JobInfo { State = 1, Id = id });
                    if (dbRes > 0)
                    {
                        return true;
                    }
                    return false;
                }

                var path = @"http://localhost:25250/api/job/?id=" + id + "&name=" + name;
                var code = HttpClientHelper.GetString(path);
                if (Convert.ToInt32(code) == 200)
                {
                    int dbRes = JobInfoDb.UpdateJobState(new JobInfo { State = 1, Id = id });
                    if (dbRes > 0)
                    {
                        return true;
                    }
                    return false;
                }

                return false;

                //int dbRes = JobInfoDb.UpdateJobState(new JobInfo { State = 1, Id = id });
                //if (dbRes > 0)
                //{
                //    var jobKey = new JobKey("ScanJob", "ScanJob");
                //    Scheduler.ResumeJob(jobKey);
                //    runRes = true;
                //}
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Pause(int id)
        {
            bool pauseRes = false;
            try
            {
                //查询数据库库
                JobInfo jobInfo = JobInfoDb.GetJobInfo(id);

                if (jobInfo?.Id > 0)
                {
                    Scheduler.PauseJob(new JobKey(jobInfo.JobName, jobInfo.JobName));
                    jobInfo.State = 2;
                    //修改数据库
                    int dbRes = JobInfoDb.UpdateJobState(jobInfo);
                    if (dbRes == 0)
                    {
                        //TODO:数据库修改失败,记录日志
                    }
                    else
                    {
                        pauseRes = true;
                    }
                }

            }
            catch (Exception e)
            {
            }

            return pauseRes;
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Resume(int id)
        {
            bool resumeRes = false;
            try
            {
                JobInfo jobInfo = JobInfoDb.GetJobInfo(id);

                if (jobInfo?.Id > 0)
                {
                    Scheduler.ResumeJob(new JobKey(jobInfo.JobName, jobInfo.JobName));
                    jobInfo.State = 1;
                    //修改数据库
                    int dbRes = JobInfoDb.UpdateJobState(jobInfo);
                    if (dbRes == 0)
                    {
                        //TODO:数据库修改失败,记录日志
                    }
                    else
                    {
                        resumeRes = true;
                    }
                }

            }
            catch (Exception e)
            {
            }

            return resumeRes;
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Remove(int id)
        {
            bool removeRes = false;
            try
            {
                JobInfo jobInfo = JobInfoDb.GetJobInfo(id);
                if (jobInfo?.Id > 0)
                {
                    JobKey jobKey = new JobKey(jobInfo.JobName, jobInfo.JobName);
                    TriggerKey triggerKey = new TriggerKey(jobInfo.JobName, jobInfo.JobName);

                    Scheduler.PauseTrigger(triggerKey);
                    Scheduler.UnscheduleJob(triggerKey);
                    Scheduler.DeleteJob(jobKey);
                    jobInfo.State = 3;
                    //修改数据库
                    int dbRes = JobInfoDb.UpdateJobState(jobInfo);
                    if (dbRes == 0)
                    {
                        //TODO:数据库修改失败,记录日志
                    }
                    else
                    {
                        removeRes = true;
                    }
                }
            }
            catch (Exception e)
            {
            }

            return removeRes;
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
                    var triggerKey = new TriggerKey(jobInfo.JobName, jobInfo.JobName);

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