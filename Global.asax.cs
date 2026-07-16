using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace NewLife
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            NewLife.Helpers.EnvLoader.CargarDotEnv();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            NewLife.Data.ResponsableData.MigrarContrasena();
            NewLife.Data.ProductoData.MigrarDescuento();
            NewLife.Data.CategoriaData.MigrarCategorias();
            NewLife.Data.VerificacionData.MigrarTablaVerificacion();
        }
    }
}
