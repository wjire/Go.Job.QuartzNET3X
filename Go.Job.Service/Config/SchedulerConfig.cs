using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Go.Job.Service.Logic.Listener;
using Quartz.Listener;

namespace Go.Job.Service.Config
{
    /// <summary>
    /// 调度器配置
    /// </summary>
    internal class SchedulerConfig
    {
        /// <summary>
        /// 调度器监听地址
        /// </summary>
        internal static string ApiAddress = ConfigurationManager.AppSettings["ApiAddress"];

        /// <summary>
        /// 调度器名称
        /// </summary>
        internal static string SchedulerName{ get; set; }

        /// <summary>
        /// job监听器
        /// </summary>
        internal static JobListenerSupport JobListener { get; set; } 

        /// <summary>
        /// 触发器监听器 
        /// </summary>
        internal static TriggerListenerSupport TriggerListener { get; set; }


        internal static void Init(string name)
        {
            SchedulerName = name;
            JobListener  = new DefaultJobListener(SchedulerName);
            TriggerListener  = new DefaultTriggerListener(SchedulerName);
        }
    }
}
