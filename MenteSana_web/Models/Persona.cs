namespace MenteSana_web.Models
{
    public class Persona
    {
        public Persona()
        {
        }

        public int id_persona {  get; set; }
        public string nombre {  get; set; }
        public string apellidos { get; set; }
        public string correo_institucional { get; set; }
        public string contrasena { get; set; }

        public string ConfirmarClave { get; set; }


    }
}
