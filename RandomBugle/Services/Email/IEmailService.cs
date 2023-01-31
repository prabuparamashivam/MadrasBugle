using System.Threading.Tasks;

namespace RandomBugle.Services.Email
{
    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string message);
    }
}
