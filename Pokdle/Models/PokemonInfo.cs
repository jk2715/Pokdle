using PokeApiNet;
using Pokdle.Infrastructure;
namespace Pokdle.Models
{
    public class PokemonInfo
    {
        public Pokemon Pokemon { get; set; }
        public PokemonSpecies Species { get; set; }
        public EvolutionChain EvolutionChain { get; set; }
        public int EvolutionStage { get; set; }
        public int EvolutionCount
        {
            get
            {
                int evolutionCount = 1;
                foreach (var evo in EvolutionChain.Chain.EvolvesTo)
                {
                    evolutionCount++;
                    foreach (var evo2 in evo.EvolvesTo)
                    {
                        evolutionCount++;
                    }
                }
                return evolutionCount;
            }
        }
        public List<string> TypeNames
        {
            get 
            { 
                return Pokemon.Types.Select(t => $"{t.Type.Name.Substring(0, 1).ToUpper()}{t.Type.Name.Substring(1, t.Type.Name.Length - 1)}").ToList();
            }
        }
        public List<string> AbilityNames
        {
            get
            {
                return Pokemon.Abilities.Select(t => $"{t.Ability.Name.Substring(0, 1).ToUpper()}{t.Ability.Name.Substring(1, t.Ability.Name.Length - 1)}").ToList();
            }
        }
        public int Generation
        {
            get
            {
                return int.Parse(Species.Generation.Url.Split("generation/")[1].Substring(0, 1));
            }
        }
        public List<string> TopBaseStats
        {
            get
            {
                var topBaseStats = Pokemon.Stats.ToDictionary(key => key.Stat.Name, val => val.BaseStat).OrderByDescending(d => d.Value).Take(2);
                return topBaseStats.Select(t => $"{t.Key.Substring(0, 1).ToUpper()}{t.Key.Substring(1, t.Key.Length - 1)}").ToList();

            }
        }
        public List<string> BottomBaseStats
        {
            get
            {
                var bottomBaseStats = Pokemon.Stats.ToDictionary(key => key.Stat.Name, val => val.BaseStat).OrderBy(d => d.Value).Take(2);
                return bottomBaseStats.Select(t => $"{t.Key.Substring(0, 1).ToUpper()}{t.Key.Substring(1, t.Key.Length - 1)}").ToList();
            }
        }
    }
}
