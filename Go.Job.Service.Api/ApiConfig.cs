using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Job.Service.Api
{
    ///
    public static class ApiConfig
    {

        /// <summary>
        /// 调度器监听地址
        /// </summary>
        public static readonly string ApiAddress = ConfigurationManager.AppSettings["ApiAddress"];


        /// <summary>
        /// 调度器名称
        /// </summary>
        public static string SchedulerName;
    }
}
