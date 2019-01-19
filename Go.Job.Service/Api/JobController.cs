using System;
using System.Web.Http;
using Go.Job.Model;
using Go.Job.Service.Config;

namespace Go.Job.Service.api
{
    /// <summary>
    /// job控制器
    /// </summary>
    public class JobController : ApiController
    {

        private static readonly string SchedName = AppSettingsConfig.SchedName;

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public Result Run(JobInfo jobInfo)
        {
            try
            {
                bool res = SchedulerManager.Singleton.CreateJob(jobInfo);
                return GetResult(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return GetResult(e.Message);
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public Result Pause(JobInfo jobInfo)
        {
            try
            {
                bool res = SchedulerManager.Singleton.Pause(jobInfo);
                return GetResult(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return GetResult(e.Message);
            }
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public Result Resume(JobInfo jobInfo)
        {
            try
            {
                bool res = SchedulerManager.Singleton.Resume(jobInfo);
                return GetResult(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return GetResult(e.Message);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]

        public Result Remove(JobInfo jobInfo)
        {
            try
            {
                bool res = SchedulerManager.Singleton.Remove(jobInfo);
                return GetResult(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return GetResult(e.Message);
            }
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public Result Update(JobInfo jobInfo)
        {
            try
            {
                bool res = SchedulerManager.Singleton.Update(jobInfo);
                return GetResult(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return GetResult(e.Message);
            }
        }

        /// <summary>
        /// 更换版本
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public Result Upgrade(JobInfo jobInfo)
        {
            try
            {
                bool res = SchedulerManager.Singleton.Upgrade(jobInfo);
                return GetResult(res);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return GetResult(e.Message);
            }
        }


        private Result GetResult(bool res)
        {
            return new Result
            {
                Code = res ? 200 : 400,
                Data = AppSettingsConfig.SchedName,
            };
        }

        private Result GetResult(string exception)
        {
            return new Result
            {
                Code = 200,
                Data = AppSettingsConfig.SchedName,
                Msg = exception
            };
        }
    }
}
