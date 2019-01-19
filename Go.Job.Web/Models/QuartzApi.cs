using System.Configuration;

namespace Go.Job.Web.Models
{

    public class QuartzApi : ConfigurationSection
    {
        public string SchedName { get; set; }


        public string ApiAddress { get; set; }
    }
}