using MenteSana_web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
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
        static string conexion = "Data Source=DESKTOP-RTLL7R5;Initial Catalog=DBMenteSana2; Integrated Security=True; TrustServerCertificate=True";
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
    }
}

