using E_Commerce.Models;
using System.Threading.Tasks;

namespace E_Commerce.services
{
    public interface Isendmail
    {
        Task SendEmailConfirmationEmail(UserEmail userEmail);
        Task SendEmailForForgotPassword(UserEmail userEmail);
        Task SendTestEmail(UserEmail userEmail);
    }
}