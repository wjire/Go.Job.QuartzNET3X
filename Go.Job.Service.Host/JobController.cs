using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Go.Job.Service.Factory;

namespace Go.Job.Service.Host
{
    public class JobController: ApiController
    {
        public void Get(int id)
        {
            JobFactory.AddJob(id);
        }
    }
}
