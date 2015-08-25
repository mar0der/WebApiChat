#region

using Microsoft.Owin;

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

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            this.ConfigureAuth(app);
            app.MapSignalR();
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