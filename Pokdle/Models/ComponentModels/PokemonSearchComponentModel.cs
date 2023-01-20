using PokeApiNet;

namespace Pokdle.Models.ComponentModels
{
    public class PokemonSearchComponentModel
    {
        public List<Pokemon> Pokemon { get; set; }

        public PokemonSearchComponentModel(List<Pokemon> pokemon)
        {
            Pokemon = pokemon;
        }
    }
}
