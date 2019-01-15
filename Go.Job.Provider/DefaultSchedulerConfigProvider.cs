using Go.Job.IProvider;
using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;

namespace Go.Job.Provider
{
    public class DefaultSchedulerConfigProvider : ISchedulerConfigProvider
    {
        public NameValueCollection GetSchedulerConfig()
        {
            NameValueCollection properties = new NameValueCollection();

            //设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.DefaultThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";

            //设置配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "555";
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp";
            properties["quartz.scheduler.exporter.channelName"] = "httpQuartz";
            properties["quartz.scheduler.exporter.rejectRemoteRequests"] = "true";



            ////===持久化===
            ////存储类型
            //properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
            ////表明前缀
            //properties["quartz.jobStore.tablePrefix"] = "QRTZ_";
            ////驱动类型
            //properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.MySQLDelegate, Quartz";
            ////数据源名称
            //properties["quartz.jobStore.dataSource"] = "myDS";
            ////连接字符串
            //properties["quartz.dataSource.myDS.connectionString"] = @"Data Source=(local);Initial Catalog=JobScheduler;User ID=sa;Password=123465";
            ////sqlserver版本
            //properties["quartz.dataSource.myDS.provider"] = "SqlServer-20";

            ////===远程调用===
            //properties["quartz.scheduler.instanceName"] = "refuge";
            //properties["quartz.scheduler.proxy"] = "true";
            //properties["quartz.scheduler.proxy.address"] = "tcp://127.0.0.1:555/QuartzScheduler";

            return properties;
        }
    }
}
