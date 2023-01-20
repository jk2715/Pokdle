using Pokdle.Models.ComponentModels;
using Microsoft.AspNetCore.Mvc;

namespace Pokdle.ViewComponents
{
    public class PokemonDropdownViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PokemonSearchComponentModel model)
        {
            return View(model);
        }
    }
}
