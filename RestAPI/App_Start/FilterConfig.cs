using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using log4net;

namespace RestAPI
{
    public class FilterConfig
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            Log.Info(filters.ToString());
            //preActionProcess();
        }

       
   
    }
}
