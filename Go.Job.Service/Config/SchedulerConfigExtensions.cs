using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Job.Service.Config
{

    public static class SchedulerConfigExtensions
    {

        public static SchedulerFactory AddThreadPoolConfig(this SchedulerFactory scf)
        {
            scf.AddThreadPoolConfig(new ThreadPoolConfig());
            return scf;
        }

        public static SchedulerFactory AddThreadPoolConfig(this SchedulerFactory scf, ThreadPoolConfig config)
        {
            scf._properties.Add(config);
            return scf;
        }

        public static SchedulerFactory AddRemoteConfig(this SchedulerFactory scf)
        {
            scf.AddRemoteConfig(new RemoteConfig());
            return scf;
        }

        public static SchedulerFactory AddRemoteConfig(this SchedulerFactory scf, RemoteConfig config)
        {
            scf._properties.Add(config);
            return scf;
        }

        public static SchedulerFactory AddJobStoreConfig(this SchedulerFactory scf)
        {
            scf.AddJobStoreConfig(new JobStoreConfig());
            return scf;
        }

        public static SchedulerFactory AddJobStoreConfig(this SchedulerFactory scf, JobStoreConfig config)
        {
            scf._properties.Add(config);
            return scf;
        }
    }
}
