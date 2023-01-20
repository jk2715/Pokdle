using Microsoft.AspNetCore.Mvc;
using Pokdle.Models.ComponentModels;
namespace Pokdle.ViewComponents
{
    public class PokemonGuessViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PokemonGuessComponentModel model)
        {
            return View(model);
        }
    }
}
