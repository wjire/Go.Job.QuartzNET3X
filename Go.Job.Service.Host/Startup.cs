using Microsoft.Owin.Cors;
using Owin;

namespace Go.Job.Service.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //允许CORS跨域
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}
