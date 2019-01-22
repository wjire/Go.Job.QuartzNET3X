using Go.Job.Model;
using Go.Job.Service.Logic;
using Go.Job.Service.Middleware;
using System;
using System.Runtime.CompilerServices;
using System.Web.Http;

namespace Go.Job.Service.Api
{
    /// <summary>
    /// job控制器
    /// </summary>
    [SchedulerNameFilter]
    public class JobController : ApiController
    {

        private static readonly ILogWriter LogWriter = (ILogWriter)MidContainer.GetService(typeof(ILogWriter));

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public Result Run(JobInfo jobInfo)
        {
            return Excute(() => SchedulerManager.Singleton.CreateJob(jobInfo));
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public Result Pause(JobInfo jobInfo)
        {
            return Excute(() => SchedulerManager.Singleton.Pause(jobInfo));
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public Result Resume(JobInfo jobInfo)
        {
            return Excute(() => SchedulerManager.Singleton.Resume(jobInfo));
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]

        public Result Remove(JobInfo jobInfo)
        {
            return Excute(() => SchedulerManager.Singleton.Remove(jobInfo));
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public Result Update(JobInfo jobInfo)
        {
            return Excute(() => SchedulerManager.Singleton.Update(jobInfo));
        }

        /// <summary>
        /// 更换版本
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public Result Upgrade(JobInfo jobInfo)
        {
          return  Excute(() => SchedulerManager.Singleton.Upgrade(jobInfo));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private Result Excute(Func<bool> func,[CallerMemberName] string method = null)
        {
            var result = new Result {Code = 200};
            try
            {
               result.Code =  func.Invoke()?200:400;
            }
            catch (Exception ex)
            {
                LogWriter.WriteException(ex, method);
                result.Msg = ex.Message;
            }
            return result;
        }
    }
}
