using System.Web;
using System.Web.Optimization;

namespace ThueXeVn
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/Scripts/core").Include(
                "~/Scripts/core.js"));

            bundles.Add(new StyleBundle("~/Content/fontend/css/main").Include(
                "~/Content/fontend/css/jquery.datetimepicker.css",
                "~/Content/fontend/css/owl.carousel.css",
                "~/Content/fontend/css/owl.theme.css",
                "~/Content/fontend/css/style.css",
                "~/Content/fontend/css/responsive.css",
                "~/Content/fontend/css/stylemh.css",
                "~/Content/fontend/css/iconmoon.css"
                ));

            bundles.Add(new ScriptBundle("~/Content/fontend/js/scripts").Include(
                "~/Content/fontend/js/owl.carousel.js",                
                "~/Content/fontend/js/main.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/main").Include("~/Scripts/script2.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
