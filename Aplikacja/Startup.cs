using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Aplikacja.Startup))]
namespace Aplikacja
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
