using System;
using System.IO;
using Go.Job.Db;
using Go.Job.Model;
using Go.Job.Web.Helper;

namespace Go.Job.Web.Logic
{
    internal class JobLogic
    {

        //TODO:暂时不用远程代理的方式.
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
        public bool Run(int id)
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
                    string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/run";
                    Result result = HttpClientHelper.PostJson<Result>(path, jobInfo);
                    if (result.Code == 200)
                    {
                        int dbRes = JobInfoDb.UpdateJobState(new JobInfo { State = 1, Id = id });
                        if (dbRes > 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        throw new Exception(result.Msg);
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
        public bool Pause(int id)
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
                    string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/pause";
                    Result result = HttpClientHelper.PostJson<Result>(path, jobInfo);
                    if (result.Code == 200)
                    {
                        int dbRes = JobInfoDb.UpdateJobState(new JobInfo { State = 2, Id = id });
                        if (dbRes > 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        throw new Exception(result.Msg);
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
        public bool Resume(int id)
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

                    string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/resume";
                    Result result = HttpClientHelper.PostJson<Result>(path, jobInfo);

                    if (result.Code == 200)
                    {
                        int dbRes = JobInfoDb.UpdateJobState(new JobInfo { State = 1, Id = id });
                        if (dbRes > 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        throw new Exception(result.Msg);
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
        public bool Remove(int id)
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
                    string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/remove";
                    Result result = HttpClientHelper.PostJson<Result>(path, jobInfo);
                    if (result.Code == 200)
                    {
                        int dbRes = JobInfoDb.UpdateJobState(new JobInfo { State = 3, Id = id });
                        if (dbRes > 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        throw new Exception(result.Msg);
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
        public bool Update(JobInfo jobInfo)
        {
            try
            {
                if (jobInfo != null && jobInfo.Id > 0 && jobInfo.State == 2)
                {
                    string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/update";
                    Result result = HttpClientHelper.PostJson<Result>(path, jobInfo);
                    if (result.Code == 200)
                    {
                        //更新成功后,状态改回来
                        jobInfo.State = 1;
                        int dbRes = JobInfoDb.UpdateJobInfo(jobInfo);
                        if (dbRes > 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        throw new Exception(result.Msg);
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
        public bool Upgrade(JobInfo jobInfo)
        {
            try
            {
                if (jobInfo != null && jobInfo.Id > 0 && (jobInfo.State == 0 || jobInfo.State == 3))
                {
                    string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/upgrade";
                    Result result = HttpClientHelper.PostJson<Result>(path, jobInfo);
                    if (result.Code == 200)
                    {
                        //更新成功后,状态改回来
                        jobInfo.State = 1;
                        int dbRes = JobInfoDb.UpdateJobInfo(jobInfo);
                        if (dbRes > 0)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        throw new Exception(result.Msg);
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