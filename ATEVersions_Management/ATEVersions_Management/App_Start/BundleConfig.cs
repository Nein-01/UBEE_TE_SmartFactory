using System.Web;
using System.Web.Optimization;

namespace ATEVersions_Management
{
    public class BundleConfig
    {        
        public static void RegisterBundles(BundleCollection bundles)
        {
            //===================== Common Bundles =====================
            //- CSS
            bundles.Add(new StyleBundle("~/Common/css").Include(
                      "~/Assets/CSS/FrameworkCSS/bootstrap.css",
                      "~/Assets/CSS/FrameworkCSS/site.css",
                      "~/Assets/Vendor/daterangepicker/daterangepicker.css",
                      "~/Assets/Vendor/datatables/dataTables.bootstrap4.min.css",
                      "~/Assets/Vendor/datatables/dtblExt/buttons.bootstrap4.min.css"));

            bundles.Add(new StyleBundle("~/lib/supportcss").Include(
                      "~/Assets/CSS/FrameworkCSS/toastr.min.css",
                      "~/Assets/Vendor/sweetalert/sweetalert.css",
                      "~/Assets/Vendor/sweetalert/sweetalert2.min.css"));

            bundles.Add(new StyleBundle("~/lib/fontawesome").Include(
                      "~/Assets/Vendor/fontawesome-free/css/all.css"));
            //- JS
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Assets/JS/FrameworkJS/jquery-3.4.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Assets/JS/FrameworkJS/jquery.validate.min.js",
                        "~/Assets/JS/FrameworkJS/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Assets/JS/FrameworkJS/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/commonjs").Include(
                      "~/Assets/JS/FrameworkJS/bootstrap.js",                      
                      "~/Assets/Vendor/bootstrap/js/bootstrap.bundle.min.js",
                      "~/Assets/Vendor/jquery-easing/jquery.easing.min.js",                      
                      "~/Assets/Vendor/datatables/jquery.dataTables.min.js",
                      "~/Assets/Vendor/datatables/dataTables.bootstrap4.min.js",
                      "~/Assets/Vendor/datatables/dtblExt/datatables.min.js",
                      "~/Assets/Vendor/datatables/dtblExt/jszip.min.js",
                      "~/Assets/Vendor/datatables/dtblExt/buttons.bootstrap4.min.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/supportjs").Include(
                      "~/Assets/JS/FrameworkJS/toastr.min.js",
                      "~/Assets/Vendor/sweetalert/sweetalert2.min.js",
                      "~/Assets/Vendor/momentjs/moment.min.js",
                      "~/Assets/Vendor/daterangepicker/daterangepicker.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/highchartjs").Include(
            "~/Assets/Vendor/highcharts_js/highcharts.js",
            "~/Assets/Vendor/highcharts_js/highcharts-more.js",
            "~/Assets/Vendor/highcharts_js/modules/accessibility.js",
            "~/Assets/Vendor/highcharts_js/modules/histogram-bellcurve.js"));

            bundles.Add(new ScriptBundle("~/Common/myjs").Include(
                      "~/Assets/JS/MyJS/NotifyManual.js",
                      "~/Assets/JS/MyJS/common_functions.js",
                      "~/Assets/JS/MyJS/customDataTables.js"));

            //===================== Admin Bundles =====================
            //- CSS
            bundles.Add(new StyleBundle("~/Admin/css").Include(
                      "~/Assets/CSS/AdminCSS/sb-admin-2.css"));

            //- JS
            bundles.Add(new ScriptBundle("~/Admin/js").Include(
                        "~/Assets/JS/AdminJS/sb-admin-2.js"));

            //===================== Client Bundles =====================
            //- CSS
            
            //- JS




        }
    }
}
