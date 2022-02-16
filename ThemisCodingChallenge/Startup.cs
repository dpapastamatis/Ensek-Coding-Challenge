using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EnsekCodingChallenge.Startup))]
namespace EnsekCodingChallenge
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
