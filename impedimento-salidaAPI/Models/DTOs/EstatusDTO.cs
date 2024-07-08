namespace impedimento_salidaAPI.Models.DTOs
{
    public class EstatusDTO
    {
        public int Id { get; set; }

        public string TipoCodigo { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public string Codigo { get; set; } = null!;
    }
}
