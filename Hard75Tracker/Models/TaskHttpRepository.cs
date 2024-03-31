using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Text.Json.Serialization;
using Hard75API.Models;
//using System.Text.Json;
using Newtonsoft.Json;

namespace Hard75Tracker.Models
{
    public class TaskHttpRepository : ITaskHttpRepository
    {
        private readonly HttpClient _client;
        public TaskHttpRepository(HttpClient client)
        {
            _client = client;
            //_options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<Hard75Shared.Response> CreateTask(Hard75Shared.Task body)
        {
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                if(body.parentTaskID == null)
                {
                    body.parentTaskID = 0;
                }
                if(body.categoryID == null)
                {
                    body.categoryID = 0;
                }
                var callResponse = await _client.PostAsJsonAsync<Hard75Shared.Task>("task/CreateTask", body);
                //NOTE: PostAsJSONAsync no longer supported for the Methods. Must change to PostAsAsync and serialize input
                var content = await callResponse.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                if (!callResponse.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
                return JsonConvert.DeserializeObject<Hard75Shared.Response>(content); ;
            }
            catch (Exception ex)
            {
                response.statusCode = 006;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Hard75Shared.Response> ReadTask(string id)
        {
            Hard75Shared.Response response = new Hard75Shared.Response();
            Console.WriteLine("Task:ReadTask");
            try
            {
                var callResponse = await _client.GetAsync("task/GetTasks?userID=" + id);
                //NOTE: PostAsJSONAsync no longer supported for the Methods. Must change to PostAsAsync and serialize input
                var content = await callResponse.Content.ReadAsStringAsync();
                Console.WriteLine(content.ToString());
                if (!callResponse.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
                return JsonConvert.DeserializeObject<Hard75Shared.Response>(content); ;
            }
            catch (Exception ex)
            {
                response.statusCode = 007;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Hard75Shared.Response> UpdateTask(Hard75Shared.Task body)
        {
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                if (body.parentTaskID == null)
                {
                    body.parentTaskID = 0;
                }
                if (body.categoryID == null)
                {
                    body.categoryID = 0;
                }
                if (body.taskDescription == null)
                {
                    body.taskDescription = "";
                }
                if(body.active == null)
                {
                    body.active = 1;
                }
                var callResponse = await _client.PostAsJsonAsync<Hard75Shared.Task>("task/UpdateTask", body);
                //NOTE: PostAsJSONAsync no longer supported for the Methods. Must change to PostAsAsync and serialize input
                var content = await callResponse.Content.ReadAsStringAsync();
                if (!callResponse.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
                return JsonConvert.DeserializeObject<Hard75Shared.Response>(content); ;
            }
            catch (Exception ex)
            {
                response.statusCode = 008;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Hard75Shared.Response> RemoveTask(Hard75Shared.Task body)
        {
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                body.active = 0;
                var callResponse = await _client.PostAsJsonAsync<Hard75Shared.Task>("task/UpdateTask", body);
                //NOTE: PostAsJSONAsync no longer supported for the Methods. Must change to PostAsAsync and serialize input
                var content = await callResponse.Content.ReadAsStringAsync();
                if (!callResponse.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
                return JsonConvert.DeserializeObject<Hard75Shared.Response>(content); ;
            }
            catch (Exception ex)
            {
                response.statusCode = 009;
                response.message = ex.Message;
                return response;
            }
        }
    }
}
