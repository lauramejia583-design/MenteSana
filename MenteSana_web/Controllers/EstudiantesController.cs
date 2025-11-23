using AspNetCoreGeneratedDocument;
using MenteSana_web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer;
//using Newtonsoft.Json; // Para serializar el objeto usuario a JSON
using System.Data;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MenteSana_web.Controllers
{
    public class EstudiantesController : Controller
    {
        static string conexion = "Data Source=DESKTOP-FS43UDE;Initial Catalog=DBMenteSana2;Integrated Security=True;";

        public IActionResult Registro_emocion()
        {
            return View();

        }

        [HttpPost]
        public IActionResult GuardarEmocion([FromBody] Models.Estado_Emociomal modelo)
        {
            try
            {
                model.Id_Estudiante = Convert.ToInt32(HttpContext.Session.GetString("id_estudiante"));

                if (id_Estudiante == null)
                {
                    return Json(new { success = false, message = "Sesión expirada." });
                }

                using (SqlConnection con = new SqlConnection(conexion))
                {
                    SqlCommand cmd = new SqlCommand("insertar_estado_emocional", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id_persona", model.Id_Estudiante);
                    cmd.Parameters.AddWithValue("@nombre_estado", modelo.nombre_estado);
                    cmd.Parameters.AddWithValue("@nota", (object)modelo.nota ?? DBNull.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
