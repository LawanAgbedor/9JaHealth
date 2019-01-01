using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using UTIL;

namespace WSApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly Logger<WebApiApplication> Log = new Logger<WebApiApplication>();

        protected void Application_Start(object sender, EventArgs e)
        {
            // Set up our logging
            //
            log4net.Config.XmlConfigurator.Configure();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var a = e;
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var a = e;
            //a = null;
            //a.ToString();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var err = Server.GetLastError();
            var a = e;

            HttpContext.Current.ClearError();
            //Response.Redirect("~/Error.aspx", false);
            Log.Fatal("An unhandled application error has occurred with exception!", null, err);
            return;
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}
