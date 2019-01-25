using System;
using System.Collections.Generic;
using System.Web.Mvc;
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
                    f.NEXT_FIRE_TIME = new DateTime(Convert.ToInt64(f.NEXT_FIRE_TIME)).AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
                    f.PREV_FIRE_TIME = new DateTime(Convert.ToInt64(f.PREV_FIRE_TIME)).AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
                    f.START_TIME = new DateTime(Convert.ToInt64(f.START_TIME)).AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
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
            jobInfo.JobGroup = jobInfo.SchedName;
            JobInfoDb.AddJobInfo(jobInfo);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        public ActionResult Run(JobInfo jobInfo)
        {
            Result res = new JobLogic().Run(jobInfo);
            return Json(res);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        public ActionResult Pause(JobInfo jobInfo)
        {
            Result result = new JobLogic().Pause(jobInfo);
            return Json(result);
        }


        /// <summary>
        /// 恢复
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        public ActionResult Resume(JobInfo jobInfo)
        {
            Result res = new JobLogic().Resume(jobInfo);
            return Json(res);
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
            Result res = new JobLogic().Update(jobInfo);
            return Json(res);
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        public ActionResult Remove(JobInfo jobInfo)
        {
            Result res = new JobLogic().Remove(jobInfo);
            return Json(res);
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