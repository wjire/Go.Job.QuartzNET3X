using Go.Job.Model;
using System;
using System.Web.Http;

namespace Go.Job.Service.WebAPI
{

    public class JobController : ApiController
    {
        [HttpGet]
        public int Run(int id)
        {
            try
            {
                return JobPoolManager.Instance.CreateJob(id) ? 200 : 400;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 400;
            }
        }

        [HttpGet]
        public int Pause(int id)
        {
            try
            {
                //return JobPoolManager.Instance.Remove(id) ? 200 : 400;
                return JobPoolManager.Instance.Pause(id) ? 200 : 400;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 400;
            }
        }


        [HttpGet]
        public int Resume(int id)
        {
            try
            {
                //return JobPoolManager.Instance.CreateJob(id) ? 200 : 400;
                return JobPoolManager.Instance.Resume(id) ? 200 : 400;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 400;
            }
        }


        [HttpGet]
        public int Remove(int id)
        {
            try
            {
                return JobPoolManager.Instance.Remove(id) ? 200 : 400;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 400;
            }
        }


        [HttpPost]
        public int Update(JobInfo jobInfo)
        {
            try
            {
                return JobPoolManager.Instance.Update(jobInfo) ? 200 : 400;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 400;
            }
        }
    }
}
