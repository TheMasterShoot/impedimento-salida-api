namespace impedimento_salidaAPI.Models.DTOs
{
    public class TipoEstatusDTO
    {
        public int Id { get; set; }

        public string Codigo { get; set; } = null!;

        public string Descripcion { get; set; } = null!;
    }
}
