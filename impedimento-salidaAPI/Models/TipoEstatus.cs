using System;
using System.Collections.Generic;

namespace impedimento_salidaAPI.Models;

public partial class TipoEstatus
{
    public int Id { get; set; }

    public string Codigo { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Estatus> Estatuses { get; set; } = new List<Estatus>();
}
