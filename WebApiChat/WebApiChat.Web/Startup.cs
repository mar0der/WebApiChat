#region

using Microsoft.Owin;

using WebApiChat.Web;

#endregion

[assembly: OwinStartup(typeof(Startup))]

namespace WebApiChat.Web
{
    #region

    using Owin;

    #endregion

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}