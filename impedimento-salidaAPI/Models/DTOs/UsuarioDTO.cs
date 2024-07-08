namespace impedimento_salidaAPI.Models.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        public int Rolid { get; set; }
        
        public string? RolDesc { get; set; }

        public int Estatusid { get; set; }
        
        public string? EstatusDesc { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Apellido { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
