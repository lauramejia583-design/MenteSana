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
                        while (dr.Read())
                        {
                            Cita c = new Cita
                            {
                                Id_Cita = Convert.ToInt32(dr["id_cita"]),

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

        public IActionResult DetalleCita(int id)
        {
            Cita cita = new Cita();

            using (SqlConnection cn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_detalle_cita", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_cita", id);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        cita.Id_Cita = Convert.ToInt32(dr["id_cita"]); 
                        cita.NombreEstudiante = dr["nombre_estudiante"].ToString();
                        cita.ApellidoEstudiante = dr["apellido_estudiante"].ToString();
                        cita.NombrePsicologo = dr["nombre_psicologo"].ToString();
                        cita.ApellidoPsicologo = dr["apellido_psicologo"].ToString();
                        cita.Fecha = Convert.ToDateTime(dr["fecha"]);
                        cita.Hora = (TimeSpan)dr["hora"];
                        cita.motivo = dr["motivo"].ToString();
                    }
                }
            }

            return View(cita);
        }


    }
}
