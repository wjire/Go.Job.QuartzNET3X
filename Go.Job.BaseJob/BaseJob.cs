using System;

namespace Go.Job.BaseJob
{
    /// <summary>
    /// 逻辑Job基类
    /// </summary>
    public abstract class BaseJob : MarshalByRefObject
    {
        /// <summary>
        /// 具体逻辑
        /// </summary>
        public abstract void Run();


        /// <summary>
        /// 将对象生存期更改为永久,避免被回收.
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
