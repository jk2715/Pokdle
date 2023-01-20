using PokeAPI;
using System.Net.Http;

namespace Pokdle.Infrastructure
{
    public class PokeClient
    {
        private readonly HttpClient _httpClient;
        public PokeClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> Get(string endpoint)
        {
            return await _httpClient.GetAsync(endpoint);
        }
    }
}
