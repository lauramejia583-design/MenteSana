using Microsoft.AspNetCore.Http;
using System.Text.Json;
using MenteSana_web.Models;

namespace MenteSana_web.Helpers
{
    public static class SessionHelper
    {
        public static Persona GetUsuario(HttpContext context)
        {
            var data = context.Session.GetString("Persona");

            if (data == null)
                return null;

            return JsonSerializer.Deserialize<Persona>(data);
        }
    }
}
