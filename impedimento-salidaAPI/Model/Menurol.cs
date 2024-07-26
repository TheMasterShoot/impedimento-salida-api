using System;
using System.Collections.Generic;

namespace impedimento_salidaAPI.Model;

public partial class Menurol
{
    public int Id { get; set; }

    public int? Idmenu { get; set; }

    public int? Idrol { get; set; }

    public virtual Menu? IdmenuNavigation { get; set; }

    public virtual Role? IdrolNavigation { get; set; }
}
