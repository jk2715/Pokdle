using Pokdle.Models.ComponentModels;
using PokeApiNet;
namespace Pokdle.Models.PageModels
{
    public class MainPageModel
    {
        public PokemonSearchComponentModel PokemonResults { get; set; }
        public PokemonGuessComponentModel PokemonGuesses { get; set; }
        public string PokemonName { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsRandomMode { get; set; }
        public int RandomSeed { get; set; }
        public string BaseUrl { get; set; }
        public MainPageModel()
        {
            PokemonResults = new PokemonSearchComponentModel(new List<PokeApiNet.Pokemon>());
        }
    }
}
