using System;
using EastWestWalk.NetFrameWork.Common.Write;

namespace Go.Job.BaseJob
{
    /// <summary>
    /// 逻辑Job基类
    /// </summary>
    public abstract class BaseJob : MarshalByRefObject
    {

        /// <summary>
        /// 运行
        /// </summary>
        /// <returns>true:运行成功;false:运行失败</returns>
        public virtual bool Run()
        {
            var res = false;
            try
            {
                Execute();
                res = true;
            }
            catch (Exception ex)
            {
                LogService.WriteLog(ex, this.GetType().Name);
            }
            return res;
        }


        /// <summary>
        /// 具体逻辑
        /// </summary>
        protected abstract void Execute();
        

        /// <summary>
        /// 将对象生存期更改为永久,因为C#默认5分钟不调用,会被回收.
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
