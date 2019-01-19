using System;
using Go.Job.Db;
using Go.Job.Model;

namespace Go.Job.Web.Helper
{
    internal static class JobHelper
    {

        //TODO:不用代理
        //public static readonly IRemotableQuartzScheduler Scheduler;

        //static JobHelper()
        //{
        //    if (Scheduler == null)
        //    {
        //        NameValueCollection properties = new NameValueCollection();
        //        properties["quartz.scheduler.instanceName"] = "调度作业监控系统";
        //        properties["quartz.scheduler.proxy"] = "true";
        //        properties["quartz.scheduler.proxy.address"] = "tcp://127.0.0.1:555/QuartzScheduler";
        //        RemotingSchedulerProxyFactory proxyFactory = new RemotingSchedulerProxyFactory
        //        {
        //            Address = "tcp://127.0.0.1:555/QuartzScheduler"
        //        };
        //        Scheduler = proxyFactory.GetProxy();
        //        if (Scheduler.IsShutdown)
        //        {
        //            Scheduler.Start();
        //        }
        //    }
        //}

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Run(int id)
        {
            try
            {
                if (id == 0)
                {
                    return false;
                }

                JobInfo jobInfo = JobInfoDb.GetJobInfo(id);
                if (jobInfo != null && jobInfo.Id == id && jobInfo.State == 0)
                {
                    //string path = @"http://localhost:25250/api/job/run?id=" + id;
                    //string code = HttpClientHelper.GetString(path);

                    string path = @"http://localhost:25250/api/job/run";
                    string code = HttpClientHelper.PostJson(path, jobInfo);
                    if (Convert.ToInt32(code) == 200)
                    {
                        int dbRes = JobInfoDb.UpdateJobState(new JobInfo { State = 1, Id = id });
                        if (dbRes > 0)
                        {
                            return true;
                        }
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

                JobInfo jobInfo = JobInfoDb.GetJobInfo(id);
                if (jobInfo != null && jobInfo.Id == id && jobInfo.State == 1)
                {
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

                JobInfo jobInfo = JobInfoDb.GetJobInfo(id);
                if (jobInfo != null && jobInfo.Id == id && jobInfo.State == 2)
                {
                    //string path = @"http://localhost:25250/api/job/resume?id=" + id;
                    //string code = HttpClientHelper.GetString(path);

                    string path = @"http://localhost:25250/api/job/resume";
                    string code = HttpClientHelper.PostJson(path, jobInfo);

                    if (Convert.ToInt32(code) == 200)
                    {
                        int dbRes = JobInfoDb.UpdateJobState(new JobInfo { State = 1, Id = id });
                        if (dbRes > 0)
                        {
                            return true;
                        }
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

                JobInfo jobInfo = JobInfoDb.GetJobInfo(id);
                if (jobInfo != null && jobInfo.Id == id && jobInfo.State == 2)
                {
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
            }
            catch (Exception e)
            {
            }

            return false;
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        public static bool Update(JobInfo jobInfo)
        {
            try
            {
                if (jobInfo != null && jobInfo.Id > 0 && jobInfo.State == 2)
                {
                    string path = @"http://localhost:25250/api/job/update";
                    string code = HttpClientHelper.PostJson(path, jobInfo);
                    if (Convert.ToInt32(code) == 200)
                    {
                        //更新成功后,状态改回来
                        jobInfo.State = 1;
                        int dbRes = JobInfoDb.UpdateJobInfo(jobInfo);
                        if (dbRes > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return false;
        }



        /// <summary>
        /// 更换版本
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        public static bool Upgrade(JobInfo jobInfo)
        {
            try
            {
                if (jobInfo != null && jobInfo.Id > 0 && (jobInfo.State == 0||jobInfo.State == 3))
                {
                    string path = @"http://localhost:25250/api/job/upgrade";
                    string code = HttpClientHelper.PostJson(path, jobInfo);
                    if (Convert.ToInt32(code) == 200)
                    {
                        //更新成功后,状态改回来
                        jobInfo.State = 1;
                        int dbRes = JobInfoDb.UpdateJobInfo(jobInfo);
                        if (dbRes > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return false;
        }
    }
}