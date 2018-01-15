using System.Web;
using System.Web.Optimization;

namespace ParsiBin.UI
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
                      "~/Scripts/respond.js",
                      "~/Scripts/jquery.bootstrap-modal-ajax-form.js",
                      "~/Scripts/jquery.bootstrap-modal-confirm.js"));

            bundles.Add(new StyleBundle("~/Content/css-Cust").Include(
                      "~/Content/bootstrap-RTL.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/css-Cust2").Include(
                    "~/Content/bootstrap.min.css",
                    "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css/font-awesome").Include("~/Content/css/font-awesome.css"));
        }
    }
}
