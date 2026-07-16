using System;
using System.IO;
using System.Web.Hosting;

namespace NewLife.Helpers
{
    // Carga variables de entorno desde un archivo .env en la raiz del proyecto
    // (no versionado, ver .gitignore) hacia Environment, para que ConexionBD
    // pueda leer POSTGRES_CONNECTION_STRING con Environment.GetEnvironmentVariable.
    public static class EnvLoader
    {
        public static void CargarDotEnv()
        {
            string ruta = HostingEnvironment.MapPath("~/.env");
            if (ruta == null || !File.Exists(ruta)) return;

            foreach (var linea in File.ReadAllLines(ruta))
            {
                var l = linea.Trim();
                if (l.Length == 0 || l.StartsWith("#")) continue;

                int igual = l.IndexOf('=');
                if (igual <= 0) continue;

                string clave = l.Substring(0, igual).Trim();
                string valor = l.Substring(igual + 1).Trim();
                if (valor.Length >= 2 && valor[0] == '"' && valor[valor.Length - 1] == '"')
                    valor = valor.Substring(1, valor.Length - 2);

                if (Environment.GetEnvironmentVariable(clave) == null)
                    Environment.SetEnvironmentVariable(clave, valor);
            }
        }
    }
}
