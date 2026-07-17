using System;
using System.IO;

namespace NewLife.Helpers
{
    // Carga variables de entorno desde un archivo .env en la raiz del proyecto
    // (no versionado, ver .gitignore) hacia Environment, para que ConexionBD y
    // demas helpers puedan leer sus variables con Environment.GetEnvironmentVariable.
    // En Render las variables se configuran directamente en el dashboard y este
    // archivo simplemente no existe, por lo que este loader no hace nada ahi.
    public static class EnvLoader
    {
        public static void CargarDotEnv()
        {
            string ruta = Path.Combine(Directory.GetCurrentDirectory(), ".env");
            if (!File.Exists(ruta)) return;

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
