using Go.Job.Db;
using Go.Job.Model;
using System;


namespace Go.Job.Logic
{

    /// <summary>
    /// JobInfo逻辑类
    /// </summary>
    public class JobInfoLogic
    {
        /// <summary>
        /// 添加Job信息
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        public int AddInfo(JobInfo jobInfo)
        {
            int result = 0;

            if (jobInfo == null)
            {
                return result;
            }

            try
            {
                result = JobInfoDb.AddJobInfo(jobInfo);
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return result;
        }

        /// <summary>
        /// 更新Job的状态
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        public int UpdateJobState(JobInfo jobInfo)
        {
            int result = 0;

            if (jobInfo == null)
            {
                return result;
            }

            try
            {
                result = UpdateJobState(jobInfo);
            }
            finally
            {

            }

            return result;
        }


        /// <summary>
        /// job数量
        /// </summary>
        /// <returns></returns>
        public int GetJobCount()
        {
            int list = 0;

            try
            {
                string SqlStr = "select count(0) from job_log";

            }
            finally
            {

            }

            return list;
        }

    }
}