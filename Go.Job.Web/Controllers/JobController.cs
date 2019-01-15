using Go.Job.Db;
using Go.Job.Model;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Go.Job.Web.Controllers
{
    public class JobController : Controller
    {
        public ActionResult Index()
        {
            List<JobInfo> list = JobInfoDb.GetJobInfoList();
            return View(list);
        }


        public ActionResult Add(JobInfo jobInfo)
        {
            JobInfoDb.AddJobInfo(jobInfo);
            return RedirectToAction("Index", "Job");
        }

        public ActionResult Run(int id)
        {
            JobInfoDb.UpdateJobState(new JobInfo { Id = id, StartTime = DateTime.Now, State = 0 });
            return RedirectToAction("Index", "Job");
        }

        public ActionResult Pause(int id)
        {
            JobInfoDb.UpdateJobState(new JobInfo { Id = id, State = 2 });
            return RedirectToAction("Index", "Job");
        }

        public ActionResult Resume(int id)
        {
            JobInfoDb.UpdateJobState(new JobInfo { Id = id, State = 1 });
            return RedirectToAction("Index", "Job");
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            JobInfo jobInfo = JobInfoDb.GetJobInfo(id);
            return Json(jobInfo, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateJob(JobInfo jobInfo)
        {
            JobInfoDb.UpdateJobState(jobInfo);
            return RedirectToAction("Index", "Job");
        }


        public ActionResult Remove(int id)
        {
            JobInfoDb.UpdateJobState(new JobInfo { Id = id, State = 5 });
            return RedirectToAction("Index", "Job");
        }
    }
}