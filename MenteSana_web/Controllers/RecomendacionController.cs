using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.Data;
using Microsoft.AspNetCore.Http;
//using Newtonsoft.Json; // Para serializar el objeto usuario a JSON
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MenteSana_web.Models;

namespace MenteSana_web.Controllers
{
    public class RecomendacionController : Controller
    {
        static string conexion = "Data Source=DESKTOP-FS43UDE;Initial Catalog=DBMenteSana2; Integrated Security=True; TrustServerCertificate=True";
        public IActionResult Recomendaciones(int id_tipo_estado)
        {
            List<Recomendacion> lista = new List<Recomendacion>();
            string emocionNombre = "";
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_obtener_recomendaciones_por_estado", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Recomendacion
                            {
                                contenido = dr["contenido"].ToString()
                            });
                        }
                    }
                }
            }

            ViewBag.Emocion = emocionNombre;
            return View(lista);
        }
    }
}