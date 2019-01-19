using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Go.Job.Service.api.Startup))]

namespace Go.Job.Service.api
{
    internal class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{Controller}/{action}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                });

            app.UseWebApi(config);
        }
    }
}

