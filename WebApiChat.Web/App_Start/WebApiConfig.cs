using System.Net.Http.Headers;
using System.Web.Http.Cors;
using System.Web.Http;
using System.Web.OData.Extensions;

namespace WebApiChat.Web
{
    #region

    using System.Web.Http;

    using Microsoft.Owin.Security.OAuth;

    #endregion

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //var cors = new EnableCorsAttribute("www.example.com", "*", "*");
           // config.EnableCors();
            //config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.AddODataQueryFilter();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));


        }
    }
}
