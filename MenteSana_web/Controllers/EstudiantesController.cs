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
        static string conexion = "Data Source=DESKTOP-RTLL7R5;Initial Catalog=DBMenteSana2; Integrated Security=True; TrustServerCertificate=True";

        public IActionResult Registro_emocion()
        {
            return View();

        }

        public IActionResult HomeEstudiantes()
        {
            return View();
        }

        public IA

        [HttpPost]
        public IActionResult GuardarEmocion(EstadoEmociomal model)
        {
            model.id_persona = Convert.ToInt32(HttpContext.Session.GetString("id_estudiante"));

            using (SqlConnection cn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_insertar_estado_emocional", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id_persona", model.id_persona);
                cmd.Parameters.AddWithValue("@nota", model.nota);
                cmd.Parameters.AddWithValue("@id_tipo_estado", model.id_tipo_estado);
                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("ERROR SQL: " + ex.Message);

                    // O mientras depuras, que reviente:
                    throw;
                }
            }

            return RedirectToAction("Registro_emocion");

        }
    }
}