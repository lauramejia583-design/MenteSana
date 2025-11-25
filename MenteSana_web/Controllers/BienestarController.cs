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
    public class BienestarController : Controller
    {
        static string conexion = "Data Source=DESKTOP-FS43UDE;Initial Catalog=DBMenteSana2; Integrated Security=True; TrustServerCertificate=True";

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult HomeBienestar()
        {
            return View();
        }
    }
}

