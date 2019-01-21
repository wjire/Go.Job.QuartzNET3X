using System.Collections.Specialized;

namespace Go.Job.Service.Config
{
    //TODO:尚未开放远程调度功能,勿用该配置 
    /// <summary>
    /// 远程调度配置
    /// </summary>
    public class RemoteConfig
    {
        public string Type { get; set; } = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";

        public string Port { get; set; } = "555";

        public string BindName { get; set; } = "QuartzScheduler";

        public string ChannelType { get; set; } = "tcp";

        public string ChannelName { get; set; } = "httpQuartz";

        public string RejectRemoteRequests { get; set; } = "true";


        public static implicit operator NameValueCollection(RemoteConfig config)
        {
            NameValueCollection properties = new NameValueCollection
            {
                ["quartz.scheduler.exporter.type"] = config.Type,
                ["quartz.scheduler.exporter.port"] = config.Port,
                ["quartz.scheduler.exporter.bindName"] = config.BindName,
                ["quartz.scheduler.exporter.channelType"] = config.ChannelType,
                ["quartz.scheduler.exporter.channelName"] = config.ChannelName,
                ["quartz.scheduler.exporter.rejectRemoteRequests"] = config.RejectRemoteRequests
            };
            return properties;
        }
    }
}
