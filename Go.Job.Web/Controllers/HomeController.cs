using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Go.Job.Db;
using Go.Job.Model;
using Go.Job.Web.Logic;

namespace Go.Job.Web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<JobPager> list = JobInfoDb.GetJobPager();

            //转换Quartz数据库表的时间字段
            list.ForEach(f =>
                {
                    f.NEXT_FIRE_TIME = new DateTime(Convert.ToInt64(f.NEXT_FIRE_TIME)).AddHours(8).ToString();
                    f.PREV_FIRE_TIME = new DateTime(Convert.ToInt64(f.PREV_FIRE_TIME)).AddHours(8).ToString();
                    f.START_TIME = new DateTime(Convert.ToInt64(f.START_TIME)).AddHours(8).ToString();
                });
            return View(list);
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        public ActionResult Add(JobInfo jobInfo)
        {
            if (!string.IsNullOrWhiteSpace(jobInfo.Cron))
            {
                jobInfo.JobGroup = jobInfo.SchedName;
                JobInfoDb.AddJobInfo(jobInfo);
            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Run(int id)
        {
            new JobLogic().Run(id);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Pause(int id)
        {
            new JobLogic().Pause(id);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Resume(int id)
        {
            new JobLogic().Resume(id);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Update(int id)
        {
            JobInfo jobInfo = JobInfoDb.GetJobInfo(id);
            return Json(jobInfo, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        public ActionResult UpdateJob(JobInfo jobInfo)
        {
            new JobLogic().Update(jobInfo);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(int id)
        {
            new JobLogic().Remove(id);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            new JobLogic().Delete(id);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Upgrade(int id)
        {
            JobInfo jobInfo = JobInfoDb.GetJobInfo(id);
            return Json(jobInfo, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        public ActionResult UpgradeJob(JobInfo jobInfo)
        {
            new JobLogic().Upgrade(jobInfo);
            return RedirectToAction("Index", "Home");
        }
    }
}