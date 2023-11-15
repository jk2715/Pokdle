using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokdle.Infrastructure;
using Pokdle.Models.ComponentModels;
using Pokdle.Models.PageModels;
using Pokdle.Repositories;
using PokeApiNet;
using Pokdle.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

namespace Pokdle.Controllers
{
    public class MainController : Controller
    {
        private readonly PokemonRepository _pokemonRepository;
        private PokemonInfo _pokemonOfTheDay;
        private static List<Pokemon> _pokemonList;
        private readonly IConfiguration _configuration;
        public MainController(IConfiguration configuration)
        {
            _pokemonRepository = _pokemonRepository != null ? _pokemonRepository : new PokemonRepository(configuration);
            _configuration = configuration;
            _pokemonOfTheDay = GetCurrentPokemon();
            _pokemonList = _pokemonList ?? _pokemonRepository.GetAllPokemon().GetAwaiter().GetResult();
        }

        public async Task<IActionResult> Index(bool isRandom = false, int seed = 0)
        {
            MainPageModel model = new MainPageModel();
           // _pokeNode = pokemonNode ?? _pokeNode;
            string cachedRandomKey = seed > 0 ? "Random" : "";
            List<PokemonInfo> pokemonGuessesInfoList = HttpContext.Session.GetComplexData<List<PokemonInfo>>($"{cachedRandomKey}PokemonInfoGuesses") ?? new List<PokemonInfo>();
            List<Guess> guesses = HttpContext.Session.GetComplexData<List<Guess>>($"{cachedRandomKey}Guesses") ?? new List<Guess>();
            seed = seed == 0 ? HttpContext.Session.GetInt32("RandomSeed") ?? seed : seed;
            _pokemonOfTheDay = seed > 0 ? GetCurrentPokemon(seed) : _pokemonOfTheDay;
            isRandom = !isRandom ? Convert.ToBoolean(HttpContext.Session.GetString("IsRandom") ?? "false") : isRandom;
            model.IsRandomMode = isRandom;
            model.IsSuccess = guesses.Any() ? guesses.Last().IsCorrect : false;
            model.RandomSeed = seed;
            model.BaseUrl = _configuration["BaseUrl"];
            model.PokemonName = _pokemonOfTheDay.Pokemon.Name.ToLowerInvariant();
            model.PokemonGuesses = new PokemonGuessComponentModel(pokemonGuessesInfoList, guesses);
            model.PokemonResults = new PokemonSearchComponentModel(_pokemonList);
            model.GenerationsList = _configuration.GetSection("GenerationsList").Get<List<string>>();
            return View(model);
        }

        public IActionResult SearchPokemon(string searchTerm, int seed, string generation)
        {
            string mode = seed > 0 ? "Random" : "";
            List<PokemonInfo> pokemonGuessesInfoList = HttpContext.Session.GetComplexData<List<PokemonInfo>>($"{mode}PokemonInfoGuesses") ?? new List<PokemonInfo>();
            var searchResults = int.TryParse(searchTerm, out int id) ? 
                Search.SearchById(id, pokemonGuessesInfoList.Select(p => p.Pokemon.Id).ToList(), _pokemonList, generation)
                :Search.SearchByName(searchTerm, pokemonGuessesInfoList.Select(p => p.Pokemon.Id).ToList(), _pokemonList, generation);         
            return ViewComponent("PokemonDropdown", new PokemonSearchComponentModel(searchResults));
        }
        public async Task<IActionResult> TryGuess(string id, int seed = 0)
        {
            PokemonInfo guessInfo = new PokemonInfo();
            _pokemonOfTheDay = seed > 0 ? await DailyPokemon.GetDailyPokemon(_pokemonRepository, int.Parse(_configuration["PokemonTotal"]), seed) : _pokemonOfTheDay;
            string cachedRandomKey = seed > 0 ? "Random" : "";
            guessInfo.Pokemon = await _pokemonRepository.GetPokemon(id);
            guessInfo.Species = await _pokemonRepository.GetPokemonSpecies(id);
            guessInfo.EvolutionChain = await _pokemonRepository.GetEvolutionChain(guessInfo.Species.EvolutionChain.Url);
            guessInfo.EvolutionStage = await _pokemonRepository.GetEvolutionStage(guessInfo.Species);
            var guess = new GuessChecker();
            var guessResults = guess.CheckGuess(guessInfo, _pokemonOfTheDay);
            List<PokemonInfo> currentInfo = HttpContext.Session.GetComplexData<List<PokemonInfo>>($"{cachedRandomKey}PokemonInfoGuesses") ?? new List<PokemonInfo>();
            List<Guess> currentGuesses = HttpContext.Session.GetComplexData<List<Guess>>($"{cachedRandomKey}Guesses") ?? new List<Guess>();
            currentInfo.Add(guessInfo);
            currentGuesses.Add(guessResults);
            HttpContext.Session.SetComplexData($"{cachedRandomKey}PokemonInfoGuesses", currentInfo);
            HttpContext.Session.SetComplexData($"{cachedRandomKey}Guesses", currentGuesses);
            return ViewComponent("PokemonGuess", new PokemonGuessComponentModel(currentInfo, currentGuesses));
        }
        public IActionResult RandomMode()
        {
            Random rnd = new Random();
            int seed = rnd.Next();
            HttpContext.Session.SetInt32("RandomSeed", seed);
            HttpContext.Session.SetString("IsRandom", "true");
            return RedirectToAction("Index", new { isRandom = true, seed = seed });
        }
        public IActionResult DailyMode()
        {
            HttpContext.Session.Remove("RandomPokemonInfoGuesses");
            HttpContext.Session.Remove("RandomGuesses");
            HttpContext.Session.SetInt32("RandomSeed", 0);
            HttpContext.Session.SetString("IsRandom", "false");
            return RedirectToAction("Index");
        }
        private PokemonInfo GetCurrentPokemon(int seed = 0)
        {
            PokemonInfo newPokemon = new PokemonInfo();

            while (newPokemon.Pokemon == null)
            {
                try
                {
                    newPokemon = DailyPokemon.GetDailyPokemon(_pokemonRepository, int.Parse(_configuration["PokemonTotal"]) + 1, seed).GetAwaiter().GetResult();
                }
                catch
                {

                }
            }
            return newPokemon;
        }
    }
}
