using System;
using System.Collections.Generic;

namespace impedimento_salidaAPI.Model;

public partial class Role
{
    public int Id { get; set; }

    public string Rol { get; set; } = null!;

    public virtual ICollection<Ciudadano> Ciudadanos { get; set; } = new List<Ciudadano>();

    public virtual ICollection<Menurol> Menurols { get; set; } = new List<Menurol>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
