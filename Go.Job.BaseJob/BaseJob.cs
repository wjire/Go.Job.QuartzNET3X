using System;
using System.Diagnostics;
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
        public bool Run()
        {
            bool res = false;
            try
            {
                LogService.WriteLog($"{DateTime.Now} : 开始执行");
                Execute();
                LogService.WriteLog($"{DateTime.Now} : 执行结束\r\n");
                res = true;
            }
            catch (Exception ex)
            {
                LogService.WriteLog(ex, GetType().Name);
            }
            return res;
        }


        /// <summary>
        /// 具体逻辑
        /// </summary>
        protected abstract void Execute();


        /// <summary>
        /// 将对象生存期更改为永久,因为CLR默认5分钟内没有通过代理发出调用,对象会实效,下次垃圾回收会释放它的内存.
        /// </summary>
        /// <returns></returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
