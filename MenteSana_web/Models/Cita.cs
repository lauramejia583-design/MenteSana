namespace MenteSana_web.Models
{
    public class Cita
    {
        public Cita()
        {
        }

        public int Id_Cita { get; set; }
        public int Id_Estudiante { get; set; }
        public int Id_Psicologo { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string NombrePsicologo { get; set; }
        public string motivo { get; set; }

    }
}
