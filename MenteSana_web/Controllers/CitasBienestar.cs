using MenteSana_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.Data;
using Microsoft.AspNetCore.Http;
//using Newtonsoft.Json; // Para serializar el objeto usuario a JSON
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Build.Framework;
using NuGet.Protocol;

namespace MenteSana_web.Controllers
{
    public class CitasBienestar : Controller
    {
        static string conexion = "Data Source=DESKTOP-FS43UDE;Initial Catalog=DBMenteSana2; Integrated Security=True; TrustServerCertificate=True";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListaCitas()
        {
            List<Cita> Citas = new List<Cita>();
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("TraerCitasBienestar", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
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

