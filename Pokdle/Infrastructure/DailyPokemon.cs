using PokeApiNet;
using Pokdle.Repositories;
using Pokdle.Models;
namespace Pokdle.Infrastructure
{
    public static class DailyPokemon
    {
        public static async Task<PokemonInfo> GetDailyPokemon(PokemonRepository pokemonRepository, int pokeCount, int seed = 0)
        {
            seed = seed > 0 ? seed : int.Parse($"{DateTime.Today.Day}{DateTime.Today.Month}{DateTime.Today.Year}");
            Random random = new Random(seed);
            var randNum = random.Next(1, pokeCount);
            PokemonInfo pokeInfo = new PokemonInfo();
            pokeInfo.Pokemon = await pokemonRepository.GetPokemon(randNum.ToString());
            pokeInfo.Species = await pokemonRepository.GetPokemonSpecies(randNum.ToString());
            if(pokeInfo.Species.EvolutionChain == null)
            {
                return new PokemonInfo();
            }
            pokeInfo.EvolutionChain = await pokemonRepository.GetEvolutionChain(pokeInfo.Species.EvolutionChain.Url);
            pokeInfo.EvolutionStage = await pokemonRepository.GetEvolutionStage(pokeInfo.Species);
            return pokeInfo;
        }
    }
}
