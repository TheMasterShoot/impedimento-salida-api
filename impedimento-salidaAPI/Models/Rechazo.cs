using System;
using System.Collections.Generic;

namespace impedimento_salidaAPI.Models;

public partial class Rechazo
{
    public int Id { get; set; }

    public int? Levantamientoid { get; set; }

    public int? Certificacionid { get; set; }

    public string Motivo { get; set; } = null!;

    public virtual CertificacionExistencium? Certificacion { get; set; }

    public virtual SolicitudLevantamiento? Levantamiento { get; set; }
}
