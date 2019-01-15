using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Job.IProvider
{
    public interface ISchedulerConfigProvider
    {
        NameValueCollection GetSchedulerConfig();
    }
}
