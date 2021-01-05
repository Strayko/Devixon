using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Responses;
using Newtonsoft.Json;

namespace DevixonApi.Data.Services
{
    public class FacebookService : IFacebookService
    {
        private readonly HttpClient _httpClient;
        
        public FacebookService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com/v2.8/")
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<FacebookLoginResponse> GetUserFromFacebookAsync(string facebookToken)
        {
            var result = await GetAsync<dynamic>(facebookToken, "me",
                "fields=first_name,last_name,email,picture.width(100).height(100)");
            
            if (result == null) throw new Exception("User from this token not exist");

            var user = new FacebookLoginResponse()
            {
                FirstName = result.first_name,
                LastName = result.last_name,
                Email = result.email,
                Picture = result.picture.data.url
            };

            return user;
        }

        public async Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");

            if (!response.IsSuccessStatusCode) return default(T);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}