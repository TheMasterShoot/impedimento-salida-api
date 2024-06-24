using System;
using System.Collections.Generic;

namespace impedimento_salidaAPI.Models;

public partial class Ciudadano
{
    public int Id { get; set; }

    public int Rolid { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Apellido { get; set; }

    public string Cedula { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<CertificacionExistencium> CertificacionExistencia { get; set; } = new List<CertificacionExistencium>();

    public virtual Role Rol { get; set; } = null!;

    public virtual ICollection<SolicitudLevantamiento> SolicitudLevantamientos { get; set; } = new List<SolicitudLevantamiento>();
}
