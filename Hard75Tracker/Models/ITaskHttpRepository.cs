using System.Net.Http.Json;
using System.Net;
using System.Net.Mail;
using Hard75Shared;

namespace Hard75Tracker.Models
{
    public interface ITaskHttpRepository
    {
        Task<Hard75Shared.Response> CreateTask(Hard75Shared.Task body);
        Task<Hard75Shared.Response> ReadTask(string id);
        Task<Hard75Shared.Response> UpdateTask(Hard75Shared.Task body);
        Task<Hard75Shared.Response> RemoveTask(Hard75Shared.Task body);
    }
}
