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
using System.Reflection;
namespace MenteSana_web.Controllers
{
    public class CitasPsicologos : Controller
    {
        static string conexion = "Data Source=DESKTOP-FS43UDE;Initial Catalog=DBMenteSana2; Integrated Security=True; TrustServerCertificate=True";
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ListaCitaPsicologica(Cita model)
        {
            List<Cita> Citas = new List<Cita>();

            model.Id_Psicologo = Convert.ToInt32(HttpContext.Session.GetString("id_estudiante"));
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_listar_cita_psicologo", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_psicologo", model.Id_Psicologo);
                cn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Cita c = new Cita
                        {
                            NombreEstudiante = dr["nombre_estudiante"].ToString(),
                            ApellidoEstudiante = dr["apellido_estudiante"].ToString(),
                            NombrePsicologo = dr["nombre_psicologo"].ToString(),
                            ApellidoPsicologo = dr["apellido_psicologo"].ToString(),
                            Fecha = Convert.ToDateTime(dr["fecha"]),
                            Hora = (TimeSpan)dr["hora"]
                        };

                        Citas.Add(c);
                    }
                }


            }
            return View(Citas);
        }
    }
}
