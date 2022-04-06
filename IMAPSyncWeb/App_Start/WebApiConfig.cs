using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace IMAPSyncWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {


            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "IMAPSyncApi",
                routeTemplate: "api/{controller}/{action}/{account}",
                defaults: new { account = RouteParameter.Optional }
            );

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

        }
    }
}