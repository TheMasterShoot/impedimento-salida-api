using System;
using System.Collections.Generic;

namespace impedimento_salidaAPI.Models;

public partial class Ca
{
    public int Id { get; set; }

    public int? Certificacionid { get; set; }

    public string Cedula { get; set; } = null!;

    public string Cas { get; set; } = null!;

    public virtual CertificacionExistencium? Certificacion { get; set; }
}
