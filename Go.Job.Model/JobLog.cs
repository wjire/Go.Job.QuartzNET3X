using System;

namespace Go.Job.Model
{
    public class JobLog
    {
        public int Id { get; set; }

        public string JobName { get; set; }

        public string JobResult { get; set; }

        public string JobException { get; set; }

        public int ExeTimes { get; set; }

        public DateTime ExeDate { get; set; }

        public int ExeState { get; set; }

        public DateTime CreateTime { get; set; }
    }
}