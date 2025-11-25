using System.Data;

namespace MenteSana_web.Models
{
    public class EstadoEmociomal
    {
        public EstadoEmociomal()
        {
        }

        public int id_persona { get; set; }
        public string nota { get; set; }
        public int id_tipo_estado { get; set; }

    }
}
