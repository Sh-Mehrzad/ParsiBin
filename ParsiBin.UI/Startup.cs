using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ParsiBin.UI.Startup))]
namespace ParsiBin.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
