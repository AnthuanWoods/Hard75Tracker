using System.Net.Http.Json;
using System.Net;
using System.Net.Mail;
using Hard75Shared;

namespace Hard75Tracker.Models
{
    public interface IUserHttpRepository
    {
        Task<Hard75Shared.Response> RegisterUser(Hard75Shared.UserAccount body);
        Task<Hard75Shared.Response> LoginUser(Hard75Shared.UserAccount body);

        Task<Hard75Shared.Response> ReadUser(string body);
        Task<bool> sendEmail(string em, string subject, string htmlString);
    }
}
