using System.Data;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using NewLife.Data;
using NewLife.Helpers;
using ApiEjemplo.Data;

namespace NewLife.Controllers
{
    /// <summary>
    /// One-time endpoint to hash existing plain-text passwords in the database.
    /// Call POST /api/migracion/hashear-contrasenas once after deploying the hashing code.
    /// After migration, this endpoint can be disabled or removed.
    /// </summary>
    [ApiController]
    [Route("api/migracion")]
    public class MigracionController : ControllerBase
    {
        [HttpPost]
        [Route("hashear-contrasenas")]
        public IActionResult HashearContrasenas()
        {
            var sb = new StringBuilder();
            int total = 0, migrados = 0;

            // Clientes
            var clientes = ClienteData.ListarClientes();
            foreach (var c in clientes)
            {
                total++;
                if (!HashHelper.EsHash(c.contrasena ?? ""))
                {
                    using (var conn = new ConexionBD())
                    {
                        conn.AgregarParametro(ParameterDirection.Input, "@correo", SqlDbType.VarChar, 100, c.correo);
                        conn.AgregarParametro(ParameterDirection.Input, "@nuevaContrasena", SqlDbType.VarChar, 255, HashHelper.Sha256(c.contrasena ?? "111111"));
                        conn.EjecutarSentencia("sp_CambiarContrasena_Cliente", true);
                    }
                    migrados++;
                }
            }
            sb.AppendLine($"Clientes: {migrados}/{clientes.Count} migrados.");

            // Administradores
            int admMig = 0;
            var admins = AdministradorData.ListarAdministradores();
            foreach (var a in admins)
            {
                total++;
                if (!HashHelper.EsHash(a.contrasena ?? ""))
                {
                    AdministradorData.ActualizarContrasena(a.cedula_adm, HashHelper.Sha256(a.contrasena ?? "Admin123"));
                    admMig++;
                }
            }
            sb.AppendLine($"Administradores: {admMig}/{admins.Count} migrados.");

            // Responsables
            int respMig = 0;
            var responsables = ResponsableData.ListarResponsables();
            foreach (var r in responsables)
            {
                total++;
                if (!HashHelper.EsHash(r.contrasena ?? ""))
                {
                    using (var conn = new ConexionBD())
                    {
                        conn.AgregarParametro(ParameterDirection.Input, "@correo", SqlDbType.VarChar, 100, r.correo);
                        conn.AgregarParametro(ParameterDirection.Input, "@nuevaContrasena", SqlDbType.VarChar, 255, HashHelper.Sha256(r.contrasena ?? "111111"));
                        conn.EjecutarSentencia("sp_CambiarContrasena_Responsable", true);
                    }
                    respMig++;
                }
            }
            sb.AppendLine($"Responsables: {respMig}/{responsables.Count} migrados.");

            // Domiciliarios — direct SQL update since no dedicated password SP exists
            int domiMig = 0;
            var domiciliarios = DomiciliarioData.ListarDomiciliarios();
            foreach (var d in domiciliarios)
            {
                total++;
                if (!HashHelper.EsHash(d.contrasena ?? ""))
                {
                    var hash = HashHelper.Sha256(d.contrasena ?? "111111");
                    using (var conn = new ConexionBD())
                    {
                        conn.EjecutarSentencia(
                            $"UPDATE domiciliario SET contrasena = '{hash}' WHERE cedula_domi = '{d.cedula_domi}'",
                            false);
                    }
                    domiMig++;
                }
            }
            sb.AppendLine($"Domiciliarios: {domiMig}/{domiciliarios.Count} migrados.");

            sb.AppendLine($"Total revisados: {total}. Migración completada.");
            return Ok(sb.ToString());
        }
    }
}
