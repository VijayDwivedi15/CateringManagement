using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CateringSite.Startup))]
namespace CateringSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
