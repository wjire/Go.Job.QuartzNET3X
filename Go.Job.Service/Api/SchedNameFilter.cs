using Go.Job.Model;
using Go.Job.Service.Core;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Go.Job.Service.Api
{
    /// <summary>
    /// 调度任务过滤器
    /// </summary>
    public class SchedNameFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //ServiceInUsed.LogWriter.WriteException(new System.Exception("123"), "222222");

            //System.Collections.Generic.Dictionary<string, object> request = actionContext.ActionArguments;

            //JobInfo jobInfo = request["jobInfo"] as JobInfo;
            //if (jobInfo == null)
            //{
            //    throw new System.Exception("入参异常!");
            //}

            //if (!SchedulerManager.Singleton.Scheduler.SchedulerName.Equals(jobInfo.SchedName))
            //{
            //    throw new System.Exception($" {SchedulerManagerFacotry.ApiAddress} 没有监听 {jobInfo.SchedName} 调度服务!");
            //}
            base.OnActionExecuting(actionContext);
        }
    }
}
