using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MobilesProject.Startup))]
namespace MobilesProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
