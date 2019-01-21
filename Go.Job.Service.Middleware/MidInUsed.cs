using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Job.Service.Middleware
{
    public class MidInUsed
    {
        public static ILogWriter LogWriter;

        static MidInUsed()
        {
            LogWriter = (ILogWriter)MidContainer.GetService(typeof(ILogWriter));
        }
    }
}
