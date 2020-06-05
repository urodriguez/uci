using System.Web;
using Hangfire.Dashboard;

namespace Infrastructure.Crosscutting.BackgroundProcessing.Hangfire
{
    public class InventAppHangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var cookies = HttpContext.Current.Request.Cookies;
            var inventAppHfDbCookie = cookies["inventApp_hf_dashboard_cookie"];

            if (inventAppHfDbCookie == null || string.IsNullOrEmpty(inventAppHfDbCookie.Value)) return false;

            return inventAppHfDbCookie.Value.Equals("1nv3nt4pp_h4ngf1r3_d4shb0rd");
        }
    }
}