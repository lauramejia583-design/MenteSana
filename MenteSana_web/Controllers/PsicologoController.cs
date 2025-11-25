using Microsoft.AspNetCore.Mvc;

namespace MenteSana_web.Controllers
{
    public class PsicologoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult HomePsicologo()
        {
            return View();
        }
    }
}
