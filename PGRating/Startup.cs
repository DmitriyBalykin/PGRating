using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PGRating.Startup))]

namespace PGRating
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
