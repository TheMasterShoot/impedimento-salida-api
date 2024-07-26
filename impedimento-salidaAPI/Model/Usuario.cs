using System;
using System.Collections.Generic;

namespace impedimento_salidaAPI.Model;

public partial class Usuario
{
    public int Id { get; set; }

    public int Rolid { get; set; }

    public int Estatusid { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Apellido { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual Estatus Estatus { get; set; } = null!;

    public virtual Role Rol { get; set; } = null!;
}
