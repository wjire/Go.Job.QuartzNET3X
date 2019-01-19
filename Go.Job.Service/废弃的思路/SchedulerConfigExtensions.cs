//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Go.Job.Service.Config
//{

//    public static class SchedulerConfigExtensions
//    {

//        public static SchedFactory AddThreadPoolConfig(this SchedFactory scf)
//        {
//            scf.AddThreadPoolConfig(new ThreadPoolConfig());
//            return scf;
//        }

//        public static SchedFactory AddThreadPoolConfig(this SchedFactory scf, ThreadPoolConfig config)
//        {
//            scf._properties.Add(config);
//            return scf;
//        }

//        public static SchedFactory AddRemoteConfig(this SchedFactory scf)
//        {
//            scf.AddRemoteConfig(new RemoteConfig());
//            return scf;
//        }

//        public static SchedFactory AddRemoteConfig(this SchedFactory scf, RemoteConfig config)
//        {
//            scf._properties.Add(config);
//            return scf;
//        }

//        public static SchedFactory AddJobStoreConfig(this SchedFactory scf)
//        {
//            scf.AddJobStoreConfig(new JobStoreConfig());
//            return scf;
//        }

//        public static SchedFactory AddJobStoreConfig(this SchedFactory scf, JobStoreConfig config)
//        {
//            scf._properties.Add(config);
//            return scf;
//        }
//    }
//}
