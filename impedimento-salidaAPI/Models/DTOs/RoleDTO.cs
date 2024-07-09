namespace impedimento_salidaAPI.Models.DTOs
{
    public class RoleDTO
    {
        public int Id { get; set; }

        public string Rol { get; set; } = null!;

        public virtual ICollection<CiudadanoDTO>? Ciudadanos { get; set; }

        public virtual ICollection<UsuarioDTO>? Usuarios { get; set; }
    }
}
