
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
            // Configuración y servicios de Web API

            // 2. Ahora EnableCorsAttribute funcionará correctamente
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

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