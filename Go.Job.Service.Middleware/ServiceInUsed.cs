using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Go.Job.Service.Middleware;

namespace Go.Job.Service.MiddlewareContainer
{
    public static class ServiceInUsed
    {
        public static ILogWriter LogWriter;

        static ServiceInUsed()
        {
            LogWriter = (ILogWriter)MidContainer.GetService(typeof(ILogWriter));
        }
    }
}
