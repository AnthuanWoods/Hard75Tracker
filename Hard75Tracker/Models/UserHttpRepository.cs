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

        public UserHttpRepository(HttpClient client){
            _client = client;
        }

        //Function calls the API to Create the user account
        public async Task<Hard75Shared.Response> RegisterUser(Hard75Shared.UserAccount obj){
            try
            {
                //Calls the API
                var response = await _client.PostAsJsonAsync<Hard75Shared.UserAccount>("api/CreateUser", obj);
                //NOTE: PostAsJSONAsync no longer supported for the Methods. Must change to PostAsAsync and serialize input
                //Reads the Content
                var content = await response.Content.ReadAsStringAsync();
                //If not a successful request throw new error
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
                return JsonConvert.DeserializeObject<Hard75Shared.Response>(content);
            } catch(Exception ex)
            {
                return null;
            }
        }

        //Function calls the API, passing the user's username and password
        public async Task<Hard75Shared.Response> LoginUser(Hard75Shared.UserAccount obj)
        {
            try {
                var response = await _client.PostAsJsonAsync<Hard75Shared.UserAccount>("api/AttemptSignin", obj);
                //NOTE: PostAsJSONAsync no longer supported for the Methods. Must change to PostAsAsync and serialize input
                var content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
                return JsonConvert.DeserializeObject<Hard75Shared.Response>(content.ToString());
            } catch(Exception ex)
            {
                return null;
            }
        }

        //Function reads the user informaation
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
                return JsonConvert.DeserializeObject<Hard75Shared.Response>(content.ToString());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Function calls the API, sending an email to the user, currently used for MFA and will be used for a password reset.
        public async Task<bool> sendEmail(string em, string subject, string htmlString)
        {
            CustomEmail obj = new CustomEmail();
            obj.emailAddress = em;
            obj.subject = subject;
            obj.body = htmlString;
            
            //JsonContent body = JsonContent.Create(obj);
            //var requestObj = JsonConvert.SerializeObject(obj);

            var response = await _client.PostAsJsonAsync<CustomEmail>("api/SendEmail", obj);
            //NOTE: PostAsJSONAsync no longer supported for the Methods. Must change to PostAsAsync and serialize input
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
                return false;
            }
            return true;
        }
    }
}
