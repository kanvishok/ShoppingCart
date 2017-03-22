using System.Web.Optimization;

namespace ShoppingCar.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                      "~/Scripts/angular/angular.min.js",
                      "~/Scripts/angular-route/angular-route.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/ShoppingCartApp").Include(
                        "~/Scripts/app/app.js",
                        "~/Scripts/app/service/service.module.js",
                        "~/Scripts/app/service/apiCaller.service.js",
                        "~/Scripts/app/basket/basket.module.js",
                        "~/Scripts/app/basket/basket.controller.js",
                        "~/Scripts/app/basket/product.component.js",
                        "~/Scripts/app/checkout/checkout.module.js",
                        "~/Scripts/app/checkout/itemsInBasket.drictive.js",
                        "~/Scripts/app/checkout/checkout.controller.js",
                        "~/Scripts/app/confirmation/confirmation.module.js",
                        "~/Scripts/app/confirmation/confirmation.controller.js"));


        }
    }
}
