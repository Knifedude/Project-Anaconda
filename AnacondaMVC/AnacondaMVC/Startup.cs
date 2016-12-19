using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AnacondaMVC.Startup))]
namespace AnacondaMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
