using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace TessPgw.Core
{
    public class HttpClientWrapper
    {
        private readonly HttpClient _client;

        public HttpClientWrapper(string baseUrl)
        {
            var config = Config.Instance;
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(int.Parse(config.Get("CONFIG_TIMEOUT") ?? "30"))
            };
            _client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }

        public T Post<T>(string endpoint, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = _client.PostAsync(endpoint, content).Result;
            var json = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API Error [{(int)response.StatusCode}]: {json}");
            }

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
