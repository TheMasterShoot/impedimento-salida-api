using System;
using System.Collections.Generic;

namespace impedimento_salidaAPI.Model;

public partial class Estatus
{
    public int Id { get; set; }

    public string TipoCodigo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string Codigo { get; set; } = null!;

    public virtual ICollection<CertificacionExistencium> CertificacionExistencia { get; set; } = new List<CertificacionExistencium>();

    public virtual ICollection<SolicitudLevantamiento> SolicitudLevantamientos { get; set; } = new List<SolicitudLevantamiento>();

    public virtual TipoEstatus TipoCodigoNavigation { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
