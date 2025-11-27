using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenteSana_web.Models
{
    public class Persona
    {
        public Persona()
        {
        }

        // ⬇⬇ SOLO LE AGREGAMOS [Key], sin cambiar el nombre
        [Key]
        public int id_persona { get; set; }

        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string correo_institucional { get; set; }
        public string contrasena { get; set; }

        // Esta casi seguro NO está en la tabla, así que:
        [NotMapped]  // 👉 No intentará guardar/leer esta columna en BD
        public string ConfirmarClave { get; set; }

        public int id_rol { get; set; }
    }
}
