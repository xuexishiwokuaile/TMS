using System.Web;
using System.Web.Optimization;

namespace UI
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new ScriptBundle("~/bundles/layui").Include(
                        "~/Content/layui/layui.js",
                        "~/Content/layui/layui.all.js"
                        ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/layui/css/layui.css"));
        }
    }
}
