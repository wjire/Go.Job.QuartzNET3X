using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Go.Job.Model;
using Go.Job.Service.Logic;

namespace Go.Job.Service.Api
{
    /// <summary>
    /// 调度任务过滤器
    /// </summary>
    public class SchedulerNameFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            System.Collections.Generic.Dictionary<string, object> request = actionContext.ActionArguments;

            JobInfo jobInfo = request["jobInfo"] as JobInfo;
            if (jobInfo == null)
            {
                throw new System.Exception("入参异常!");
            }

            if (!ApiConfig.SchedulerName.Equals(jobInfo.SchedName))
            {
                throw new System.Exception($" 该服务没有监听 {jobInfo.SchedName} 调度任务");
            }

            base.OnActionExecuting(actionContext);
        }
    }
}

