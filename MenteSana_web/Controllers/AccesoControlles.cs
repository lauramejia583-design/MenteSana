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
    public class AccesoController : Controller
    {
        static string conexion = "Data Source=DESKTOP-FS43UDE;Initial Catalog=DBMenteSana2; Integrated Security=True; TrustServerCertificate=True";
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Registro()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        public static string ConvertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }

            }
            return sb.ToString();
        }

        [HttpPost]
        public IActionResult Login(Persona oUsuario)
        {
            oUsuario.contrasena = ConvertirSha256(oUsuario.contrasena);
            using (SqlConnection cn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("validar_usuario_web", cn);
                cmd.Parameters.AddWithValue("correo_institucional", oUsuario.correo_institucional);
                cmd.Parameters.AddWithValue("contrasena", oUsuario.contrasena);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cn.Open();
                oUsuario.id_persona = Convert.ToInt32(cmd.ExecuteScalar());


            }
            if (oUsuario.id_persona != 0)
            {
                HttpContext.Session.SetString("Persona", JsonSerializer.Serialize(oUsuario));
                HttpContext.Session.SetString("id_estudiante", oUsuario.id_persona.ToString());
                return RedirectToAction("AgendarCita", "Citas");
                //return View();
            }
            else
            {
                ViewData["Mensaje"] = "Usuario no encontrado";
                return View();
            }
        }


        //cerrar sesion
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear(); // Elimina todos los valores de sesión
            return RedirectToAction("Login", "Acceso");
        }


    }
}
