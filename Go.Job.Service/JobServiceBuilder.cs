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
    /// 
    /// </summary>
    public static class JobServiceBuilder
    {
        
        public static IJobService BuilderProduce()
        {
            return new ProduceJobService();
        }


        public static IJobService BuilderTest()
        {
            return new TestJobService();
        }
    }
}
