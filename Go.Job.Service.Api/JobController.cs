using System;
using System.Web.Http;
using Go.Job.Model;
using Go.Job.Service.Logic;
using Go.Job.Service.MiddlewareContainer;

namespace Go.Job.Service.Api
{
    /// <summary>
    /// job控制器
    /// </summary>
    //[SchedNameFilter]
    public class JobController : ApiController
    {

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
                ServiceInUsed.LogWriter.WriteException(e, nameof(Run));
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
                ServiceInUsed.LogWriter.WriteException(e, nameof(Pause));
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
                ServiceInUsed.LogWriter.WriteException(e, nameof(Resume));
                return GetResult(e.Message);
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]

        public Result Remove(JobInfo jobInfo)
        {
            try
            {
                ServiceInUsed.LogWriter.WriteException(new Exception("111111"), "Remove");
                bool res = SchedulerManager.Singleton.Remove(jobInfo);
                return GetResult(res);
            }
            catch (Exception e)
            {
                ServiceInUsed.LogWriter.WriteException(e, nameof(Remove));
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
                ServiceInUsed.LogWriter.WriteException(e, nameof(Update));
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
                ServiceInUsed.LogWriter.WriteException(e, nameof(Upgrade));
                return GetResult(e.Message);
            }
        }


        private Result GetResult(bool res)
        {
            return new Result
            {
                Code = res ? 200 : 400,
            };
        }

        private Result GetResult(string exception)
        {
            return new Result
            {
                Code = 200,
                Msg = exception
            };
        }
    }
}
