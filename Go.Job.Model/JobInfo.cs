using System;

namespace Go.Job.Model
{

    public class JobInfo
    {
        public int Id { get; set; }

        public string JobName { get; set; }

        public string Cron { get; set; } = string.Empty;

        public int Second { get; set; }

        public string AssemblyPath { get; set; } 

        public string ClassTypePath { get; set; } 

        public DateTime StartTime { get; set; }

        /// <summary>
        /// 任务状态 0:准备中 1:执行中 2:暂停 3:停止
        /// </summary>
        public int State { get; set; }

        public DateTime CreateTime { get; set; }
    }



}
