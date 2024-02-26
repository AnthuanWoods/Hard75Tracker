using System.Net.Http.Json;
using System.Net;
using System.Net.Mail;
using Hard75Shared;
using BootstrapBlazor.Components;

namespace Hard75Tracker.Models
{
    public interface ICategoryHttpRepository
    {
        Task<Hard75Shared.Response> CreateCategory(Hard75Shared.Category body);
        Task<Hard75Shared.Response> ReadCategory(string id);
        Task<Hard75Shared.Response> UpdateCategory(Hard75Shared.Category body);
        Task<Hard75Shared.Response> RemoveCategory(Hard75Shared.Category body);
    }
}
