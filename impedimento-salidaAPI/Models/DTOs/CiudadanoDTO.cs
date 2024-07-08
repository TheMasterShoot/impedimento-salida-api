namespace impedimento_salidaAPI.Models.DTOs
{
    public class CiudadanoDTO
    {
        public int Id { get; set; }

        public int Rolid { get; set; }
        
        public string? RolDesc { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Apellido { get; set; }

        public string Cedula { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Telefono { get; set; }

        public string Password { get; set; } = null!;
    }
}
