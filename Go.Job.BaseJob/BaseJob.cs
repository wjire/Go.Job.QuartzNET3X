using System;

namespace Go.Job.BaseJob
{
    /// <summary>
    /// 基Job
    /// </summary>
    public abstract class BaseJob : MarshalByRefObject, IDisposable
    {
        public abstract void Run();
        public void Dispose()
        {

        }
    }
}
