namespace impedimento_salidaAPI.Models.DTOs
{
    public class SolicitudLevantamientoDTO
    {
        public int Id { get; set; }

        public int Ciudadanoid { get; set; }

        public int Estatusid { get; set; }

        public string? EstatusDesc { get; set; }

        public string Cedula { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public DateOnly? FechaSolicitud { get; set; }

        public DateOnly? FechaAprobacion { get; set; }

        public string? UsuarioAprobacion { get; set; }

        public string? Reporte { get; set; }

        public string? LevantamientoTipo { get; set; }

        public string? Email { get; set; }

        public IFormFile? Carta { get; set; }

        public IFormFile? Sentencia { get; set; }

        public IFormFile? NoRecurso { get; set; }

        public virtual ICollection<RechazoDTO>? Rechazos { get; set; }
    }
}
