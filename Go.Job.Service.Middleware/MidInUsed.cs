using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Job.Service.Middleware
{

    /// <summary>
    /// 这个类感觉有点鸡肋..暂时没想到更优雅的方式...
    /// </summary>
    public class MidInUsed
    {
        public static ILogWriter LogWriter;

        static MidInUsed()
        {
            LogWriter = (ILogWriter)MidContainer.GetService(typeof(ILogWriter));
        }
    }
}
