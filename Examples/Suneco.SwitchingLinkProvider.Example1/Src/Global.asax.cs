

namespace Suneco.SwitchingLinkProvider.Example1
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Contains routing tables settings 
    /// </summary>
    /// <seealso cref="System.Web.HttpApplication" />
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// Applications the start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
