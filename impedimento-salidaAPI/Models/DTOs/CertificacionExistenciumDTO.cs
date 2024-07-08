﻿namespace impedimento_salidaAPI.Models.DTOs
{
    public class CertificacionExistenciumDTO
    {
        public int Id { get; set; }

        public int Ciudadanoid { get; set; }

        public int Estatusid { get; set; }

        public string? EstatusDesc { get; set; }

        public string Cas { get; set; } = null!;

        public string Cedula { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public DateOnly FechaSolicitud { get; set; }

        public string Carta { get; set; } = null!;

        public DateOnly? FechaAprobacion { get; set; }

        public string? UsuarioAprobacion { get; set; }

        public string? Reporte { get; set; }

        public string? Impedimento { get; set; }
    }
}
