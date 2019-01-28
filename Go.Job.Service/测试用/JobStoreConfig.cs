using System.Collections.Specialized;

namespace Go.Job.Service.Config
{

    /// <summary>
    /// 持久化配置
    /// </summary>
    public class JobStoreConfig
    {

        /// <summary>
        /// 序列化类型
        /// </summary>
        public string SerializerType { get; set; } = "json";

        /// <summary>
        /// 持久化类型
        /// </summary>
        public string JobStoreType { get; set; } = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";

        /// <summary>
        /// 数据库表前缀
        /// </summary>
        public string TablePrefix { get; set; } = "QRTZ_";

        /// <summary>
        /// 持久化委托类型
        /// </summary>
        public string DriverDelegateType { get; set; } = "Quartz.Impl.AdoJobStore.MySQLDelegate, Quartz";

        /// <summary>
        /// 数据源
        /// </summary>
        public string DataSource { get; set; } = "myDS";

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; } = "server=localhost;database=myquartz;user=root;pwd=admin";

        /// <summary>
        /// 数据库
        /// </summary>
        public string Provider { get; set; } = "MySql";

        /// <summary>
        /// 调度器Id是否自动配置
        /// </summary>
        public string InstanceId { get; set; } = "AUTO";


        /// <summary>
        /// 调度器名称
        /// </summary>
        public string InstanceName { get; set; }


        /// <summary>
        /// 哑火忍耐时间,单位:毫秒
        /// </summary>
        public string MisfireThreshold { get; set; } = "2000";


        /// <summary>
        /// 是否配置集群
        /// </summary>
        public string Clustered { get; set; } = "true";


        public JobStoreConfig(string schedulerName)
        {
            InstanceName = schedulerName;
        }


        public static implicit operator NameValueCollection(JobStoreConfig config)
        {
            NameValueCollection properties = new NameValueCollection
            {
                ["quartz.serializer.type"] = config.SerializerType,
                ["quartz.jobStore.type"] = config.JobStoreType,
                ["quartz.jobStore.tablePrefix"] = config.TablePrefix,
                ["quartz.jobStore.driverDelegateType"] = config.DriverDelegateType,
                ["quartz.jobStore.dataSource"] = config.DataSource,
                [$"quartz.dataSource.{config.DataSource}.connectionString"] = config.ConnectionString,
                [$"quartz.dataSource.{config.DataSource}.provider"] = config.Provider,
                ["quartz.jobStore.misfireThreshold"] = config.MisfireThreshold,
                ["quartz.jobStore.clustered"] = config.Clustered,
                ["quartz.scheduler.instanceId"] = config.InstanceId,
                ["quartz.scheduler.instanceName"] = config.InstanceName,
            };
            return properties;
        }
    }
}
