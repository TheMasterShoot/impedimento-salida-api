using impedimento_salidaAPI.Models.DTOs;

namespace impedimento_salidaAPI.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);
    }
}
