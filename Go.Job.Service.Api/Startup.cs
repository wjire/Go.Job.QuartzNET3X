using System.Net.Http.Formatting;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Go.Job.Service.Api.Startup))]

namespace Go.Job.Service.Api
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
            //将默认xml返回数据格式改为json
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("datatype", "json", "application/json"));
            app.UseWebApi(config);
        }
    }
}

