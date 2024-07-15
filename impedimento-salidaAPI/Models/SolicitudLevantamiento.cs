using System;
using System.Collections.Generic;

namespace impedimento_salidaAPI.Models;

public partial class SolicitudLevantamiento
{
    public int Id { get; set; }

    public int Ciudadanoid { get; set; }

    public int Estatusid { get; set; }

    public string Cedula { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public DateOnly FechaSolicitud { get; set; }

    public DateOnly? FechaAprobacion { get; set; }

    public string? UsuarioAprobacion { get; set; }

    public string? Reporte { get; set; }

    public string? LevantamientoTipo { get; set; }

    public string? Email { get; set; }

    public string? Carta { get; set; }

    public string? Sentencia { get; set; }

    public string? NoRecurso { get; set; }

    public virtual Ciudadano? Ciudadano { get; set; } = null!;

    public virtual Estatus? Estatus { get; set; } = null!;

    public virtual ICollection<Rechazo> Rechazos { get; set; } = new List<Rechazo>();
}
