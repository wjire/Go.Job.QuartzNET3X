using System;
using Go.Job.Db;
using Go.Job.Model;
using Go.Job.Web.Helper;
using System.Collections.Generic;
using System.Web.Mvc;
using Go.Job.Web.Logic;

namespace Go.Job.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<JobInfo> list = JobInfoDb.GetJobInfoList();
            list.ForEach(f =>
                {
                    f.NEXT_FIRE_TIME = new DateTime(Convert.ToInt64(f.NEXT_FIRE_TIME)).AddHours(8).ToString();
                    f.PREV_FIRE_TIME = new DateTime(Convert.ToInt64(f.PREV_FIRE_TIME)).AddHours(8).ToString();
                    f.START_TIME = new DateTime(Convert.ToInt64(f.START_TIME)).AddHours(8).ToString();
                });
            return View(list);
        }


        public ActionResult Add(JobInfo jobInfo)
        {
            if (!string.IsNullOrWhiteSpace(jobInfo.Cron))
            {
                jobInfo.JobGroup = jobInfo.SchedName;
                JobInfoDb.AddJobInfo(jobInfo);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Run(int id)
        {
            new JobLogic().Run(id);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Pause(int id)
        {
            new JobLogic().Pause(id);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Resume(int id)
        {
            new JobLogic().Resume(id);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            JobInfo jobInfo = JobInfoDb.GetJobInfo(id);
            return Json(jobInfo, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateJob(JobInfo jobInfo)
        {
            new JobLogic().Update(jobInfo);
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Remove(int id)
        {
            new JobLogic().Remove(id);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult Upgrade(int id)
        {
            JobInfo jobInfo = JobInfoDb.GetJobInfo(id);
            return Json(jobInfo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpgradeJob(JobInfo jobInfo)
        {
            new JobLogic().Upgrade(jobInfo);
            return RedirectToAction("Index", "Home");
        }
    }
}