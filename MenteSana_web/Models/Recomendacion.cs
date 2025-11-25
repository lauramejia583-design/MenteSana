using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace MenteSana_web.Models
{
    public class Recomendacion
    {
        public Recomendacion()
        {
        }

        public int Id_recomendacion { get; set; }
        public int Id_tipo_estado { get; set; }
        public string contenido {  get; set; }
        public DateTime fecha_generada { get; set; }
    }
}
