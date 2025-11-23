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
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using MenteSana_web.Helpers;

namespace MenteSana_web.Controllers
{
    public class CitasController : Controller
    {
        static string conexion = "Data Source=DESKTOP-FS43UDE;Initial Catalog=DBMenteSana2; Integrated Security=True; TrustServerCertificate=True";
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AgendarCita()
        {
            List<Persona> psicologos = new List<Persona>();

            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("Listar_Psicologos", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    psicologos.Add(new Persona
                    {
                        id_persona = Convert.ToInt32(dr["id_persona"]),
                        nombre = dr["nombres"].ToString(),
                        apellidos = dr["apellidos"].ToString()
                    });
                }
            }

            ViewBag.Psicologos = psicologos;
            return View();
        }


        // POST: Guardar cita
        [HttpPost]
        public IActionResult AgendarCita(Cita model)
        {
            model.Id_Estudiante = Convert.ToInt32(HttpContext.Session.GetString("id_estudiante"));

            using (SqlConnection cn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Insertar_Cita", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id_estudiante", model.Id_Estudiante);
                cmd.Parameters.AddWithValue("@id_psicologo", model.Id_Psicologo);
                cmd.Parameters.AddWithValue("@fecha", model.Fecha);
                cmd.Parameters.AddWithValue("@hora", model.Hora);
                cmd.Parameters.AddWithValue("@motivo", model.motivo);
                
                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                } catch (SqlException ex)
                {
                    ViewBag.mensaje = "Esta fecha no está disponible";
                }
            }

            TempData["mensaje"] = "¡Cita agendada correctamente!";
            return RedirectToAction("AgendarCita");
        }
    }
}

