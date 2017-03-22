using System.Web.Http;
using Microsoft.Owin;
using Owin;
using ShoppingCart.Api.App_Start;

[assembly: OwinStartup(typeof(ShoppingCart.Api.Startup))]

namespace ShoppingCart.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            var config = new HttpConfiguration();
            AutoMapperConfiguration.Configure();

            //var container = DependencyConfig.Register(config);
            //app.UseAutofacMiddleware(container);
            //app.UseAutofacWebApi(config);
        }
    }
}
