using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VNScience.Startup))]
namespace VNScience
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
