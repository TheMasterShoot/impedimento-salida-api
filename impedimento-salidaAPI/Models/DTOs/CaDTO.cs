namespace impedimento_salidaAPI.Models.DTOs
{
    public class CaDTO
    {
        public int Id { get; set; }

        public int? Certificacionid { get; set; }

        public string Cedula { get; set; } = null!;

        public string Cas { get; set; } = null!;
    }
}
