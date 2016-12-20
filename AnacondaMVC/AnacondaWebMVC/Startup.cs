using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AnacondaWebMVC.Startup))]
namespace AnacondaWebMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
