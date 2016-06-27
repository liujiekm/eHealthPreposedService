using eHPS.API.Filter;
using System.Web;
using System.Web.Mvc;

namespace eHPS.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            
        }
    }
}
