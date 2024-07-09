namespace impedimento_salidaAPI.Models.DTOs
{
    public class EstatusDTO
    {
        public int Id { get; set; }

        public string TipoCodigo { get; set; } = null!;

        public string? TipoDesc { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public string Codigo { get; set; } = null!;

        public virtual ICollection<CertificacionExistenciumDTO>? CertificacionExistencia { get; set; }

        public virtual ICollection<SolicitudLevantamientoDTO>? SolicitudLevantamientos { get; set; }

        public virtual ICollection<UsuarioDTO>? Usuarios { get; set; }
    }
}
