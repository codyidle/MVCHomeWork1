using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CodyMVC5HomeWork1.Startup))]
namespace CodyMVC5HomeWork1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
