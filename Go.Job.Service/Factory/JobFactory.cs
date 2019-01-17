using Go.Job.Db;
using System;

namespace Go.Job.Service.Factory
{
    public static class JobFactory
    {
        public static bool AddJob(int id)
        {
            var res = false;
            try
            {
                var jobInfo = JobInfoDb.GetJobInfo(id);
                var jobRuntimeInfo = JobPoolManager.Instance.CreateJobRuntimeInfo(jobInfo);
                res = JobPoolManager.Instance.Add(id, jobRuntimeInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return res;
        }
    }
}
