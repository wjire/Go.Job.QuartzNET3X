using System;
using System.Net.Http;
using System.Net.Http.Headers;
using EastWestWalk.NetFrameWork.Common.Write;
using Go.Job.Db;
using Go.Job.Model;
using Go.Job.Web.Helper;
using Newtonsoft.Json;

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
                string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/run";
                string res = PostJosn(path, jobInfo);
                return ProcessResult(JsonConvert.DeserializeObject<Result>(res));
            }
            catch (Exception ex)
            {
                LogService.WriteLog(ex, "启动 job 失败");
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
                string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/pause";
                string res = PostJosn(path, jobInfo);
                return ProcessResult(JsonConvert.DeserializeObject<Result>(res));
            }
            catch (Exception ex)
            {
                LogService.WriteLog(ex, "Pause job 失败");
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
                string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/resume";
                string res = PostJosn(path, jobInfo);
                return ProcessResult(JsonConvert.DeserializeObject<Result>(res));
            }
            catch (Exception ex)
            {
                LogService.WriteLog(ex, "Resume job 失败");
            }

            return false;
        }



        /// <summary>
        /// 停止
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
                string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/remove";
                string res = PostJosn(path, jobInfo);
                return ProcessResult(JsonConvert.DeserializeObject<Result>(res));
            }
            catch (Exception ex)
            {
                LogService.WriteLog(ex, "Remove job 失败");
            }

            return false;
        }




        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    return false;
                }

                JobInfoDb.DeleteJobInfo(new JobInfo { IsDeleted = 1, Id = id });
            }
            catch (Exception ex)
            {
                LogService.WriteLog(ex, "Delete job 失败");
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
                string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/update";
                string res = PostJosn(path, jobInfo);
                return ProcessResult(JsonConvert.DeserializeObject<Result>(res), () => JobInfoDb.UpdateJobInfo(jobInfo));
            }
            catch (Exception ex)
            {
                LogService.WriteLog(ex, "Update job 失败");

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
                JobInfoDb.UpdateJobInfo(jobInfo);
            }
            catch (Exception ex)
            {
                LogService.WriteLog(ex, "Upgrade job 失败");

            }
            return false;
        }


        /// <summary>
        /// 处理调度服务器返回的结果
        /// </summary>
        /// <param name="result"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private bool ProcessResult(Result result, Func<int> func = null)
        {
            if (result.Code == 200)
            {
                if (func == null || func() > 0)
                {
                    return true;
                }
            }
            throw new Exception(result.Msg);
        }


        private static string PostJosn(string path, JobInfo value)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PostAsJsonAsync<JobInfo>(path, value).Result;
                string str = response.Content.ReadAsStringAsync().Result;
                LogService.SaveLog("123", null, str);
                return str;
            }
        }
    }
}