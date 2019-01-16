using System.Collections.Generic;
using Go.Job.Model;

namespace Go.Job.Service.Helper
{
    public class MyTask<T> where T : BaseJobInfo
    {
        public List<T> GetJobInfoList<T>()
        {
            return null;
        }
    }
}
