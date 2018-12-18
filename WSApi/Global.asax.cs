using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using UTIL;

namespace AppWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly Logger<WebApiApplication> Log = new Logger<WebApiApplication>();

        protected void Application_Start()
        {
            // Set up our logging
            //
            log4net.Config.XmlConfigurator.Configure();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
