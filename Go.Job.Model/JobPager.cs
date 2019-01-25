using System;

namespace Go.Job.Model
{ 
    public class JobPager
    {

        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// job名称
        /// </summary>
        public string JobName { get; set; }


        /// <summary>
        /// job组
        /// </summary>
        public string JobGroup { get; set; }


        /// <summary>
        /// 时间表达式
        /// </summary>
        public string Cron { get; set; } = string.Empty;


        /// <summary>
        /// 间隔时间
        /// </summary>
        public int Second { get; set; }


        /// <summary>
        /// 程序集物理路径,含文件名,扩展名
        /// </summary>
        public string AssemblyPath { get; set; }


        /// <summary>
        /// 作业实例完全限定名
        /// </summary>
        public string ClassType { get; set; }


        /// <summary>
        /// job开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        


        /// <summary>
        /// job创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }


        /// <summary>
        /// 项目组
        /// </summary>
        public string ProjectTeam { get; set; } = string.Empty;


        /// <summary>
        /// 调度任务名称
        /// </summary>
        public string SchedName { get; set; } = string.Empty;


        /// <summary>
        /// 持久化状态
        /// </summary>
        public string TRIGGER_STATE { get; set; }


        /// <summary>
        /// 下次执行时间
        /// </summary>
        public string NEXT_FIRE_TIME { get; set; }


        /// <summary>
        /// 上一次执行时间
        /// </summary>
        public string PREV_FIRE_TIME { get; set; }


        /// <summary>
        /// 本次调度开始时间
        /// </summary>
        public string START_TIME { get; set; }

    }
}
