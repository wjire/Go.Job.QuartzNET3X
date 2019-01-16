using System;

namespace Go.Job.BaseJob
{
    /// <summary>
    /// 基Job
    /// </summary>
    public abstract class MarshalByRefJob : MarshalByRefObject
    {
        public abstract void Run();

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
