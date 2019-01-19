using System;
using System.Web.Http;
using Go.Job.Model;

namespace Go.Job.Service.WebAPI
{

    public class JobController : ApiController
    {
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public int Run(JobInfo jobInfo)
        {
            try
            {
                return SchedulerManager.Instance.CreateJob(jobInfo) ? 200 : 400;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 400;
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public int Pause(int id)
        {
            try
            {
                return SchedulerManager.Instance.Pause(id) ? 200 : 400;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 400;
            }
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        public int Resume(JobInfo jobInfo)
        {
            try
            {
                return SchedulerManager.Instance.Resume(jobInfo) ? 200 : 400;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 400;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public int Remove(int id)
        {
            try
            {
                return SchedulerManager.Instance.Remove(id) ? 200 : 400;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 400;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public int Update(JobInfo jobInfo)
        {
            try
            {
                return SchedulerManager.Instance.Update(jobInfo) ? 200 : 400;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 400;
            }
        }

        /// <summary>
        /// 更换版本
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public int Upgrade(JobInfo jobInfo)
        {
            try
            {
                return SchedulerManager.Instance.Upgrade(jobInfo) ? 200 : 400;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 400;
            }
        }
    }
}
