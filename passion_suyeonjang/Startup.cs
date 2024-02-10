using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(passion_suyeonjang.Startup))]
namespace passion_suyeonjang
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
