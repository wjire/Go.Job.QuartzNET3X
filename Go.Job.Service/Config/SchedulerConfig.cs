using System.Collections.Specialized;

namespace Go.Job.Service.Config
{
    public class SchedulerThreadPoolConfig
    {

        public NameValueCollection Properties { get; set; } = new NameValueCollection();


        public SchedulerThreadPoolConfig()
        {
            //设置线程池
            Properties["quartz.threadPool.type"] = "Quartz.Simpl.DefaultThreadPool, Quartz";
            Properties["quartz.threadPool.threadCount"] = "5";
            Properties["quartz.threadPool.threadPriority"] = "Normal";
        }
    }

    public class SchedulerRemoteExporterConfig
    {
        public NameValueCollection Properties { get; set; } = new NameValueCollection();

        public SchedulerRemoteExporterConfig()
        {
            //设置远程配置
            Properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            Properties["quartz.scheduler.exporter.port"] = "555";
            Properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            Properties["quartz.scheduler.exporter.channelType"] = "tcp";
            Properties["quartz.scheduler.exporter.channelName"] = "httpQuartz";
            Properties["quartz.scheduler.exporter.rejectRemoteRequests"] = "true";
        }
    }


    public class SchedulerJobStoreConfig
    {

        public NameValueCollection Properties { get; set; } = new NameValueCollection();

        public SchedulerJobStoreConfig()
        {

            //===持久化=== 有问题,勿用

            Properties["quartz.serializer.type"] = "binary";
            //Properties["quartz.serializer.type"] = "json";

            //存储类型
            Properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
            //表明前缀
            Properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
            //驱动类型
            Properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.MySQLDelegate, Quartz";
            //数据源名称
            Properties["quartz.jobStore.dataSource"] = "myDS";
            //连接字符串
            Properties["quartz.dataSource.myDS.connectionString"] = "server=localhost;database=test;user=root;pwd=admin";
            //server版本
            Properties["quartz.dataSource.myDS.provider"] = "MySql";

            Properties["quartz.scheduler.instanceId"] = "AUTO";


            //集群配置
            Properties["quartz.jobStore.clustered"] = "true";
        }
    }
}
