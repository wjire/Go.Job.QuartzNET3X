using Go.Job.Db;
using Go.Job.Model;

namespace Go.Job.Service.Factory
{
    public static class JobFactory
    {
        public  static void AddJob(int id)
        {
            var jobInfo = JobInfoDb.GetJobInfo(id);
            var jobRuntimeInfo = JobPoolManager.Instance.AddJob(jobInfo);
        }
    }
}
