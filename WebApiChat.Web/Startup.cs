﻿#region

using Microsoft.Owin;

using WebApiChat.Web;

#endregion

[assembly: OwinStartup(typeof(Startup))]

namespace WebApiChat.Web
{
    #region

    using Microsoft.AspNet.SignalR;
    using Microsoft.Owin.Cors;

    using Owin;

    #endregion

    // new code
    [assembly: OwinStartup(typeof(Startup))]
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Branch the pipeline here for requests that start with "/signalr"
            this.ConfigureAuth(app);
            app.Map(
                "/signalr", 
                map =>
                    {
                        // Setup the CORS middleware to run before SignalR.
                        // By default this will allow all origins. You can 
                        // configure the set of origins and/or http verbs by
                        // providing a cors options with a different policy.
                        map.UseCors(CorsOptions.AllowAll);
                        var hubConfiguration = new HubConfiguration();

                        // Run the SignalR pipeline. We're not using MapSignalR
                        // since this branch already runs under the "/signalr"
                        // path.
                        map.RunSignalR(hubConfiguration);
                    });

            // app.MapSignalR();

            // app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(GlobalConfiguration.Configuration);
        }
    }
}