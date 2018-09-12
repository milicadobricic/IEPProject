using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IEPProject.Startup))]
namespace IEPProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
