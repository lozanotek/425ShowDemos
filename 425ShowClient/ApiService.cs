using System;
using System.Net.Http;
using System.Net.Http.Json;

using System.Threading.Tasks;

namespace Four25ShowClient
{
    public static class ApiService
    {

        private static HttpClient GetHttpClient(string accessToken)
        {
            var apiAddress = ClientApplication.GetApiAddress();
            var httpClient = new HttpClient { BaseAddress = new Uri(apiAddress) };
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            return httpClient;
        }

        public static async Task<string> GetApiContents(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "identity");
            
            using (var httpClient = GetHttpClient(accessToken))
            { 
                var response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
        }

        public static async Task<string[]> GetRolesAsync(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "roles");

            using (var httpClient = GetHttpClient(accessToken))
            {
                var response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadFromJsonAsync<string[]>();

                return content;
            }
        }
    }
}
