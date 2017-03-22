using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ShoppingCart.Api.App_Start;
using System.IO;

namespace ShoppingCart.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(DependencyConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            Exception unhandledException = Server.GetLastError();
            string createText = "Hello and Welcome" + Environment.NewLine;
            File.WriteAllText(@"C:\Learning\test.txt", createText);

            // Open the file to read from. 
          //  string readText = File.ReadAllText(path);
        }
    }
}
