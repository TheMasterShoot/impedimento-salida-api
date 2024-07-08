namespace impedimento_salidaAPI.Models.DTOs
{
    public class SesionDTO
    {
        public int Rolid { get; set; }

        public int RolDesc { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Apellido { get; set; }

        public string Username { get; set; } = null!;
    }
}
