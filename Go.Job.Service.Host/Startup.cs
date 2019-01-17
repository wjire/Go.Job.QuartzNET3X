using System.Web.Http;
using Microsoft.Owin.Cors;
using Owin;

namespace Go.Job.Service.Host
{
    public class Startup
    {

        //signalr
        //public void Configuration(IAppBuilder app)
        //{
        //    //允许CORS跨域
        //    app.UseCors(CorsOptions.AllowAll);
        //    app.MapSignalR();
        //}


        //webapi
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{Controller}/{id}",
                defaults: new
                {
                    id = RouteParameter.Optional
                });

            app.UseWebApi(config);
        }
    }
}
