using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Go.Job.Model;
using Go.Job.Service.Api;
using Go.Job.Service.Config;
using Go.Job.Service.Logic;
using Quartz.Impl;

namespace Go.Job.Service
{
    /// <summary>
    /// 调度服务创建者
    /// </summary>
    public static class JobServiceBuilder
    {
        /// <summary>
        /// 创建一个正式的服务
        /// </summary>
        /// <returns></returns>
        public static IJobService BuilderProduce()
        {
            return new ProduceJobService();
        }


        /// <summary>
        /// 创建一个测试服务
        /// </summary>
        /// <returns></returns>
        public static IJobService BuilderTest()
        {
            return new TestJobService();
        }
    }
}
