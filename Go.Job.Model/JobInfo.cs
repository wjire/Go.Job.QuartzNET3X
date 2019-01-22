using System;

namespace Go.Job.Model
{
    
    public class JobInfo
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
        /// 间隔时间,单位:秒
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
        /// 项目组
        /// </summary>
        public string ProjectTeam { get; set; } = string.Empty;


        /// <summary>
        /// 调度任务名称
        /// </summary>
        public string SchedName { get; set; } = string.Empty;


        /// <summary>
        /// job创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }


        /// <summary>
        /// 是否删除 0:否 1:是
        /// </summary>
        public int IsDeleted { get; set; }

    }
}
