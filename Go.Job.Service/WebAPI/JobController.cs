using Go.Job.Service.Factory;
using System.Web.Http;

namespace Go.Job.Service.WebAPI
{
    public class JobController : ApiController
    {
        [HttpGet]
        public int Run(int id)
        {
            return JobFactory.AddJob(id) ? 200 : 400;
        }
    }
}
