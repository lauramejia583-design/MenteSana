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

                // Obtener nombre del estado emocional
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT nombre_estado FROM Tipo_Estado_Emocional WHERE id_tipo_estado = @id_tipo_estado", cn))
                {
                    cmd.Parameters.AddWithValue("@id_tipo_estado", id_tipo_estado);
                    emocionNombre = cmd.ExecuteScalar()?.ToString() ?? "Estado no encontrado";
                }

                // Obtener recomendaciones por SP
                using (SqlCommand cmd = new SqlCommand("sp_obtener_recomendaciones_por_estado", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_tipo_estado", id_tipo_estado);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Recomendacion
                            {
                                contenido = dr["contenido"].ToString(),
                                fecha_generada = Convert.ToDateTime(dr["fecha_generada"]),
                                Id_tipo_estado = id_tipo_estado
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
