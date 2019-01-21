using EastWestWalk.NetFrameWork.Common.Write;
using Go.Job.Db;
using Go.Job.Model;
using Go.Job.Web.Helper;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

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
                //if (jobInfo != null && jobInfo.Id == id && jobInfo.State == 0)
                //{
                string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/run";
                var res = PostJosn(path, jobInfo);
                return ProcessResult(JsonConvert.DeserializeObject<Result>(res));
                //Result result = HttpClientHelper.PostJson<Result>(path, jobInfo);
                //ProcessResult(result);


                //}
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
                //if (jobInfo != null && jobInfo.Id == id && jobInfo.State == 1)
                //{
                string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/pause";
                //string json = HttpWebrequestHelper.PostJson(path, JsonConvert.SerializeObject(jobInfo));

                //var result = JsonConvert.DeserializeObject<Result>(json);

                var res = PostJosn(path, jobInfo);
                return ProcessResult(JsonConvert.DeserializeObject<Result>(res));
                //}
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
                //if (jobInfo != null && jobInfo.Id == id && jobInfo.State == 2)
                //{

                string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/resume";
                Result result = HttpClientHelper.PostJson<Result>(path, jobInfo);
                ProcessResult(result);
                //}
            }
            catch (Exception e)
            {
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
                //if (jobInfo != null && jobInfo.Id == id && jobInfo.State == 2)
                //{
                string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/remove";
                var res = PostJosn(path, jobInfo);
                return ProcessResult(JsonConvert.DeserializeObject<Result>(res));
                //}
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
                //if (jobInfo != null && jobInfo.Id > 0 && jobInfo.State == 2)
                //{
                string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/update";
                var res = PostJosn(path, jobInfo);
                return ProcessResult(JsonConvert.DeserializeObject<Result>(res), () => JobInfoDb.UpdateJobInfo(jobInfo));
                //}
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
                //if (jobInfo != null && jobInfo.Id > 0 && (jobInfo.State == 0 || jobInfo.State == 3))
                //{
                //string path = ApiAddressHelper.GetApiAddress(jobInfo.SchedName) + "/api/job/upgrade";
                //Result result = HttpClientHelper.PostJson<Result>(path, jobInfo);
                //ProcessResult(result, () => JobInfoDb.UpdateJobInfo(jobInfo));
                JobInfoDb.UpdateJobInfo(jobInfo);
                //}
            }
            catch (Exception e)
            {

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


        private static string PostJosn<T>(string path, T value)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PostAsJsonAsync(path, value).Result;
                var str = response.Content.ReadAsStringAsync().Result;
                LogService.SaveLog("123", null, str);
                return str;
            }
        }
    }
}