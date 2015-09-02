#region

using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using WebApiChat.Web;

#endregion

[assembly: OwinStartup(typeof(Startup))]

namespace WebApiChat.Web
{
    #region

    using System.Reflection;
    using System.Web.Http;

    using Ninject;
    using Ninject.Web.Common.OwinHost;
    using Ninject.Web.WebApi.OwinHost;

    using Owin;

    using WebApiChat.Data;
    using WebApiChat.Data.Interfaces;
    using WebApiChat.Data.Repositories;

    #endregion
    //new code
    [assembly: OwinStartup(typeof(Startup))]
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            // Branch the pipeline here for requests that start with "/signalr"
         
            this.ConfigureAuth(app);
            app.Map("/signalr", map =>
            {
                // Setup the CORS middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    // EnableJSONP = true
                };
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfiguration);
            });

            //app.MapSignalR();

            //app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(GlobalConfiguration.Configuration);
        }

        //private static StandardKernel CreateKernel()
        //{
        //    var kernel = new StandardKernel();
        //    kernel.Load(Assembly.GetExecutingAssembly());

        //    RegisterMappings(kernel);
        //    return kernel;
        //}

        //public static void RegisterMappings(StandardKernel kernel)
        //{
        //    kernel.Bind<IChatData>()
        //        .To<WebApiChatData>()
        //        .WithConstructorArgument("context", c => new WebApiChatDbContext());
        //}
    }
}