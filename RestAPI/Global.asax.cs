using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RestAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();

        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Request.RequestContext.HttpContext.Items["timer"] = DateTime.Now.Ticks;
            if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["GUID"]))
            {
                HttpContext.Current.Request.Headers.Add("GUID", Guid.NewGuid().ToString());
            }
            else
            {
                if (Log.IsDebugEnabled)
                    Log.Debug("URL=[" + this.Request.Url + " ], Already exist GUID in header");
            }
            Log.InfoFormat("URL=[{0}], Method=[{1}], GUID=[{2}], timer={3} ============= BEGIN",
                this.Request.Url,
                this.Request.RequestType,
                HttpContext.Current.Request.Headers["GUID"].ToString(),
                Request.RequestContext.HttpContext.Items["timer"]
                );
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            try
            {
                long elapsedTicks = DateTime.Now.Ticks - (long)this.Request.RequestContext.HttpContext.Items["timer"];
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["RestDuration"]))
                {
                    HttpContext.Current.Request.Headers.Add("RestDuration", elapsedSpan.TotalMilliseconds.ToString());
                }
                else
                {
                    HttpContext.Current.Request.Headers["RestDuration"] = elapsedSpan.TotalMilliseconds.ToString();
                }

                Log.InfoFormat("URL=[{0}], Method=[{1}], duration time process=[{2}](ms) ============= END",
                    this.Request.Url,
                    this.Request.RequestType,
                    elapsedSpan.TotalMilliseconds.ToString()
                    );
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("URL=[{0}], Method=[{1}], exception=[{2}] ============= END",
                    this.Request.Url,
                    this.Request.RequestType,
                    ex
                    );
            }
        }
    }
}
