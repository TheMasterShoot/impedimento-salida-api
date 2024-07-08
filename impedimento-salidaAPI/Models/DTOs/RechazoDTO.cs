namespace impedimento_salidaAPI.Models.DTOs
{
    public class RechazoDTO
    {
        public int Id { get; set; }

        public int? Levantamientoid { get; set; }

        public int? Certificacionid { get; set; }

        public string Motivo { get; set; } = null!;
    }
}
