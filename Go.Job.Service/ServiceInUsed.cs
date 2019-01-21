using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Job.Service
{
    public static class ServiceInUsed
    {
        public static ILogWriter LogWriter;

        static ServiceInUsed()
        {
            LogWriter = (ILogWriter)ServiceContainer.GetService(typeof(ILogWriter));
        }
    }
}
