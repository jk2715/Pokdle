using PokeApiNet;
using Pokdle.Repositories;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace Pokdle.Infrastructure
{
    public class Search
    {
        public static List<Pokemon> SearchByName(string input, List<int> pokemonToExclude, List<Pokemon> allPokemon, string generation="All")
        {
            var results = new List<Pokemon>();
            var genFilter = FilterByGeneration(generation);
            input = input == null ? "" : $"{input.Substring(0, 1).ToUpper()}{input.Substring(1, input.Length - 1)}";
            return input == "*" ? allPokemon.Select(p => p).Where(p => !pokemonToExclude.Contains(p.Id) && genFilter.Contains(p.Id)).ToList() 
                : allPokemon.Select(p => p).Where(p => p.Name.StartsWith(input) && !pokemonToExclude.Contains(p.Id) && genFilter.Contains(p.Id)).ToList();
        }
        public static List<Pokemon> SearchById(int id, List<int> pokemonToExclude, List<Pokemon> allPokemon, string generation="All")
        {
            var results = new List<Pokemon>();
            var genFilter = FilterByGeneration(generation);
            return allPokemon.Select(p => p).Where(p => p.Id.ToString().StartsWith(id.ToString()) && !pokemonToExclude.Contains(p.Id) && genFilter.Contains(p.Id)).ToList();
        }
        private static List<int> FilterByGeneration(string gen)
        {
            List<int> idsToInclude = new List<int>();
            switch (gen)
            {
                case "I":
                    idsToInclude.AddRange(Enumerable.Range(1, 151).ToList());
                    break;
                case "II":
                    idsToInclude.AddRange(Enumerable.Range(152, 100).ToList());
                    break;
                case "III":
                    idsToInclude.AddRange(Enumerable.Range(252, 135).ToList());
                    break;
                case "IV":
                    idsToInclude.AddRange(Enumerable.Range(387, 107).ToList());
                    break;
                case "V":
                    idsToInclude.AddRange(Enumerable.Range(494, 156).ToList());
                    break;
                case "VI":
                    idsToInclude.AddRange(Enumerable.Range(650, 72).ToList());
                    break;
                case "VII":
                    idsToInclude.AddRange(Enumerable.Range(722, 88).ToList());
                    break;
                case "VIII":
                    idsToInclude.AddRange(Enumerable.Range(810, 96).ToList());
                    break;
                case "IX":
                    idsToInclude.AddRange(Enumerable.Range(906, 112).ToList());
                    break;
                default:
                    idsToInclude.AddRange(Enumerable.Range(1, 1017).ToList());
                    break;
            }
            return idsToInclude;
        }
    }
}
