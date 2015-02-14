using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StripeWithCustomersAndCharges.Startup))]
namespace StripeWithCustomersAndCharges
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
