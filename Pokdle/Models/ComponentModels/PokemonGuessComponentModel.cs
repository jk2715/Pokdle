using Pokdle.Models;
namespace Pokdle.Models.ComponentModels
{
    public class PokemonGuessComponentModel
    {
        public List<PokemonInfo> PokemonInfo { get; set; }
        public List<Guess> Guesses { get; set; }
        public bool Success
        {
            get
            {
                return Guesses.Select(g => g).Where(g => g.IsCorrect).Any();
            }
        }
        public PokemonGuessComponentModel(List<PokemonInfo> info, List<Guess> guesses)
        {
            PokemonInfo = info;
            Guesses = guesses;
        }
    }
}
