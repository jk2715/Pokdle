using Pokdle.Models;
using PokeApiNet;
namespace Pokdle.Infrastructure
{
    public class GuessChecker
    {
        public Guess CheckGuess(PokemonInfo pokemonGuess, PokemonInfo pokemonOfTheDay)
        {
            Guess guess = new Guess();
            guess.IsCorrect = pokemonOfTheDay.Pokemon.Id == pokemonGuess.Pokemon.Id;
            guess.Gen = pokemonOfTheDay.Generation == pokemonGuess.Generation ? 1 : pokemonOfTheDay.Generation > pokemonGuess.Generation ? 0 : -1;
            guess.Type = CompareTypes(pokemonGuess, pokemonOfTheDay);
            guess.EvolutionCount = pokemonGuess.EvolutionCount == pokemonOfTheDay.EvolutionCount ? 1 : pokemonGuess.EvolutionCount > pokemonOfTheDay.EvolutionCount ? -1 : 0;
            guess.Abilities = CompareAbilities(pokemonGuess, pokemonOfTheDay);
            guess.Top2BaseStats = CompareBaseStats(pokemonGuess.TopBaseStats, pokemonOfTheDay.TopBaseStats);
            guess.Bottom2BaseStats = CompareBaseStats(pokemonGuess.BottomBaseStats, pokemonOfTheDay.BottomBaseStats);
            guess.EvolutionStage = pokemonOfTheDay.EvolutionStage == pokemonGuess.EvolutionStage ? 1 : pokemonOfTheDay.EvolutionStage > pokemonGuess.EvolutionStage ? 0 : -1;
            return guess;
        }
        private int CompareTypes(PokemonInfo pokemonGuess, PokemonInfo pokemonOfTheDay)
        {
            int count = 0;
            foreach (var type in pokemonGuess.Pokemon.Types)
            {
                foreach(var type2 in pokemonOfTheDay.Pokemon.Types)
                {
                    count = type.Type.Name == type2.Type.Name ? count + 1 : count;
                }
            }
            return pokemonOfTheDay.Pokemon.Types.Count == pokemonGuess.Pokemon.Types.Count && count == pokemonOfTheDay.Pokemon.Types.Count ? 1 : count == 0 ? -1 : 0;
        }
        private int CompareAbilities(PokemonInfo pokemonGuess, PokemonInfo pokemonOfTheDay)
        {
            int count = 0;
            foreach(var ability in pokemonGuess.Pokemon.Abilities)
            {
                foreach (var ability2 in pokemonOfTheDay.Pokemon.Abilities)
                {
                    count = ability.Ability.Name == ability2.Ability.Name ? count + 1 : count;
                }
            }
            return pokemonOfTheDay.Pokemon.Abilities.Count == pokemonGuess.Pokemon.Abilities.Count && count == pokemonOfTheDay.Pokemon.Abilities.Count ? 1 : count == 0 ? -1 : 0;
        }
        private int CompareBaseStats(List<string> guessBaseStats, List<string> baseStatsOfTheDay) 
        {
            var topBaseStatsOfTheDay = string.Join(":", baseStatsOfTheDay);
            return topBaseStatsOfTheDay.Contains(guessBaseStats[0]) && topBaseStatsOfTheDay.Contains(guessBaseStats[1]) ? 
                1 : topBaseStatsOfTheDay.Contains(guessBaseStats[0]) || topBaseStatsOfTheDay.Contains(guessBaseStats[1]) ? 0 : -1;
        }
    }
}
