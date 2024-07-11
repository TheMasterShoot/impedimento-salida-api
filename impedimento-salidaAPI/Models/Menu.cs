using System;
using System.Collections.Generic;

namespace impedimento_salidaAPI.Models;

public partial class Menu
{
    public int Id { get; set; }

    public string? Descripcion { get; set; }

    public string? Icono { get; set; }

    public string? Url { get; set; }

    public virtual ICollection<Menurol> Menurols { get; set; } = new List<Menurol>();
}
