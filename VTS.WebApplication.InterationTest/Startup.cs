using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VTS.WebApplication.InterationTest.Startup))]
namespace VTS.WebApplication.InterationTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
