using System.Net;
using System.Net.Http.Json;
using System.Net.Mail;

using System.Text.Json.Serialization;
using Hard75API.Models;


//using System.Text.Json;
using Newtonsoft.Json;

namespace Hard75Tracker.Models
{
    public class UserHttpRepository : IUserHttpRepository
    {
        private readonly HttpClient _client;

        //private readonly JsonSerializerOptions _options;

        public UserHttpRepository(HttpClient client){
            _client = client;
            //_options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<Hard75Shared.Response> RegisterUser(Hard75Shared.UserAccount obj){
            try
            {
                JsonContent body = JsonContent.Create(obj);
                var requestObj = JsonConvert.SerializeObject(obj);
                var response = await _client.PostAsJsonAsync<Hard75Shared.UserAccount>("api/CreateUser", obj);
                //NOTE: PostAsJSONAsync no longer supported for the Methods. Must change to PostAsAsync and serialize input
                var content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
                return JsonConvert.DeserializeObject<Hard75Shared.Response>(content);
                //var products = JsonSerializer.Deserialize<UserAccount>(content, _options);
                //return products;
            } catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<Hard75Shared.Response> LoginUser(Hard75Shared.UserAccount obj)
        {
            try {
                JsonContent body = JsonContent.Create(obj);
                var requestObj = JsonConvert.SerializeObject(obj);
                var response = await _client.PostAsJsonAsync<Hard75Shared.UserAccount>("api/AttemptSignin", obj);
                //NOTE: PostAsJSONAsync no longer supported for the Methods. Must change to PostAsAsync and serialize input
                var content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
                System.Console.WriteLine(content.ToString());
                return JsonConvert.DeserializeObject<Hard75Shared.Response>(content.ToString());
            } catch(Exception ex)
            {
                return null;
            }
            //return JsonConvert.DeserializeObject<UserAccount>(content);
            //var products = JsonSerializer.Deserialize<UserAccount>(content, _options);
            //return products;
        }

        public async Task<Hard75Shared.Response> ReadUser(string obj)
        {
            try
            {
                var response = await _client.GetAsync("api/ViewUser?email=" + obj);
                //NOTE: PostAsJSONAsync no longer supported for the Methods. Must change to PostAsAsync and serialize input
                var content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
                System.Console.WriteLine(content.ToString());
                return JsonConvert.DeserializeObject<Hard75Shared.Response>(content.ToString());
            }
            catch (Exception ex)
            {
                return null;
            }
            //return JsonConvert.DeserializeObject<UserAccount>(content);
            //var products = JsonSerializer.Deserialize<UserAccount>(content, _options);
            //return products;
        }

        public async Task<bool> sendEmail(string em, string subject, string htmlString)
        {
            CustomEmail obj = new CustomEmail();
            obj.emailAddress = em;
            obj.subject = subject;
            obj.body = htmlString;
            
            JsonContent body = JsonContent.Create(obj);
            Console.WriteLine(body.ToString());
            var requestObj = JsonConvert.SerializeObject(obj);

            Console.WriteLine(requestObj.ToString()); 
            var response = await _client.PostAsJsonAsync<CustomEmail>("api/SendEmail", obj);
            //NOTE: PostAsJSONAsync no longer supported for the Methods. Must change to PostAsAsync and serialize input
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(content);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
                return false;
            }
            return true;
            //var products = JsonSerializer.Deserialize<UserAccount>(content, _options);
            //return products;
        }


    }
}
