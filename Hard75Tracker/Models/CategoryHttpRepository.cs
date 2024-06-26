﻿using System.Net;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Text.Json.Serialization;
using Hard75API.Models;
//using System.Text.Json;
using Newtonsoft.Json;

namespace Hard75Tracker.Models
{
    public class CategoryHttpRepository : ICategoryHttpRepository
    {
        private readonly HttpClient _client;
        public CategoryHttpRepository(HttpClient client)
        {
            _client = client;
            //_options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<Hard75Shared.Response> CreateCategory(Hard75Shared.Category body)
        {
            Console.WriteLine(body);
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                var callResponse = await _client.PostAsJsonAsync<Hard75Shared.Category>("category/CreateCategory", body);
                //NOTE: PostAsJSONAsync no longer supported for the Methods. Must change to PostAsAsync and serialize input
                var content = await callResponse.Content.ReadAsStringAsync();
                if (!callResponse.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
                return JsonConvert.DeserializeObject<Hard75Shared.Response>(content); ;
            } catch(Exception ex)
            {
                response.statusCode = 006;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Hard75Shared.Response> ReadCategory(string id)
        {
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                var callResponse = await _client.GetAsync("category/GetCategories?userID="+id);
                //NOTE: PostAsJSONAsync no longer supported for the Methods. Must change to PostAsAsync and serialize input
                var content = await callResponse.Content.ReadAsStringAsync();
                if (!callResponse.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
                return JsonConvert.DeserializeObject<Hard75Shared.Response>(content); ;
            }
            catch(Exception ex)
            {
                response.statusCode = 007;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Hard75Shared.Response> UpdateCategory(Hard75Shared.Category body)
        {
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                var callResponse = await _client.PostAsJsonAsync<Hard75Shared.Category>("category/UpdateCategory",body);
                //NOTE: PostAsJSONAsync no longer supported for the Methods. Must change to PostAsAsync and serialize input
                var content = await callResponse.Content.ReadAsStringAsync();
                if (!callResponse.IsSuccessStatusCode)
                {
                    throw new ApplicationException(content);
                }
                return JsonConvert.DeserializeObject<Hard75Shared.Response>(content); ;
            }
            catch(Exception ex)
            {
                response.statusCode = 008;
                response.message = ex.Message;
                return response;
            }
        }

        public async Task<Hard75Shared.Response> RemoveCategory(Hard75Shared.Category body)
        {
            Hard75Shared.Response response = new Hard75Shared.Response();
            try
            {
                body.active = 0;
                var callResponse = await _client.PostAsJsonAsync<Hard75Shared.Category>("category/UpdateCategory", body);
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
