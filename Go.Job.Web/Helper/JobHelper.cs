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
            }
        }


        public static bool Run(int id)
        {

            bool runRes = false;
            try
            {
                int dbRes = JobInfoDb.UpdateJobState(new JobInfo { State = 1, Id = id });
                if (dbRes > 0)
                {
                    Scheduler.ResumeJob(new JobKey("ScanJob", "ScanJob"));
                    runRes = true; 
                }
            }
            catch (Exception e)
            {

            }
            return runRes;
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
                if (jobInfo.State != 3)
                {
                    bool removeRes = Remove(jobInfo.Id);
                    if (removeRes == false)
                    {
                        return false;
                    }
                }

                int dbRes = JobInfoDb.UpdateJobInfo(jobInfo);
                if (dbRes == 0)
                {
                    //TODO:数据库修改失败,记录日志
                }
                else
                {
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