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
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Request.RequestContext.HttpContext.Items["timer"] = Stopwatch.StartNew();
            //HttpApplication httpApp = (HttpApplication)sender;
            //httpApp.Context.Items["Timer"] = Stopwatch.StartNew();
            if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["GUID"]))
            {
                HttpContext.Current.Request.Headers.Add("GUID", Guid.NewGuid().ToString());
            }
            else
            {
                Log.Debug("URL=[" + this.Request.Url + " ], Already exist GUID in header");
            }
            Log.InfoFormat("URL=[{0}], Method=[{1}], GUID=[{2}] ============= BEGIN",
                this.Request.Url,
                this.Request.RequestType,
                HttpContext.Current.Request.Headers["GUID"].ToString()
                );
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            Stopwatch timer = (Stopwatch)this.Request.RequestContext.HttpContext.Items["timer"];
            timer.Stop();
            if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["RestDuration"]))
            {
                HttpContext.Current.Request.Headers.Add("RestDuration", timer.ElapsedMilliseconds.ToString());
            }
            else
            {
                HttpContext.Current.Request.Headers["RestDuration"] = timer.ElapsedMilliseconds.ToString();
            }

            Log.InfoFormat("URL=[{0}], Method=[{1}], duration time process=[{2}](ms) ============= END",
                this.Request.Url,
                this.Request.RequestType,
                timer.ElapsedMilliseconds.ToString()
                );

        }
    }
}
