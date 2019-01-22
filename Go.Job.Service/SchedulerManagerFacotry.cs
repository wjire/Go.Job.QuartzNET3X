using Go.Job.Service.Config;
using Go.Job.Service.Listener;
using Go.Job.Service.Logic;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace Go.Job.Service
{

    /// <summary>
    /// 调度器启动类
    /// </summary>
    public sealed class SchedulerManagerFacotry
    {
        public static SchedulerManager CreateSchedulerManager()
        {
            SchedulerManager.Singleton.Scheduler = new StdSchedulerFactory().GetScheduler().Result;
            SchedulerConfig.SchedulerName = SchedulerManager.Singleton.Scheduler.SchedulerName;
            return SchedulerManager.Singleton;
        }
    }
}
