using PokeApiNet;
using Pokdle.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Pokdle.Repositories
{
    public class PokemonRepository
    {
        private readonly string _endpoint;
        private readonly IConfiguration _configuration;
        private readonly PokeClient _pokeClient;
        public PokemonRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _endpoint = _configuration["PokeApi:Endpoint"];
            _pokeClient = new PokeClient();
        }

        public async Task<List<Pokemon>> GetAllPokemon()
        {
            var response = await _pokeClient.Get($"{_endpoint}/pokemon?limit={_configuration["PokemonTotal"]}");
            var parsedResponse = await response.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<JObject>(parsedResponse);
            var parsedResults = JsonConvert.DeserializeObject<List<Pokemon>>(results["results"].ToString());
            List<Pokemon> pokeList = new List<Pokemon>();
            foreach (string name in parsedResults.Select(p => p.Name))
            {
                var poke = await GetPokemon(name);
                poke.Name = $"{poke.Name.Substring(0, 1).ToUpper()}{poke.Name.Substring(1, poke.Name.Length - 1)}";
                pokeList.Add(poke);
            }
            return pokeList;
        }

        public async Task<Pokemon> GetPokemon(string idOrName)
        {
            var response = await _pokeClient.Get($"{_endpoint}/pokemon/{idOrName}");
            var parsedResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Pokemon>(parsedResponse);
        }

        public async Task<PokemonSpecies> GetPokemonSpecies(string idOrName)
        {
            var response = await _pokeClient.Get($"{_endpoint}/pokemon-species/{idOrName}");
            var parsedResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PokemonSpecies>(parsedResponse);
        }
        public async Task<EvolutionChain> GetEvolutionChain(string url)
        {
            var response = await _pokeClient.Get(url);
            var parsedResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<EvolutionChain>(parsedResponse);
        }
        public async Task<string> Get(string url)
        {
            var response = await _pokeClient.Get(url);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<int> GetEvolutionStage(PokemonSpecies species)
        {
            int evolutionStage = 1;
            string id = species.EvolvesFromSpecies == null ? "" : species.EvolvesFromSpecies.Name;
            while (!string.IsNullOrEmpty(id))
            {
                evolutionStage++;
                var previousEvo = await GetPokemonSpecies(id);
                id = previousEvo.EvolvesFromSpecies == null ? "" : previousEvo.EvolvesFromSpecies.Name;
            }
            return evolutionStage;
        }
    }
}
