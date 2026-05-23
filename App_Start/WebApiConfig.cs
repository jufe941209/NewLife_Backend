
using System.Web.Http.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace NewLife
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // CORS
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Forzar JSON siempre — eliminar el formatter XML por completo
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(
                new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));

            // Rutas de Web API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}