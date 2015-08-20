#region

using Microsoft.Owin;

using WebApiChat.Services;

#endregion

[assembly: OwinStartup(typeof(Startup))]

namespace WebApiChat.Services
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