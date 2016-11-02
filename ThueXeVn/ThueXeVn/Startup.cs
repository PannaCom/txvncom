using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ThueXeVn.Startup))]
namespace ThueXeVn
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
