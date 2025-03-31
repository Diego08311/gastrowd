using Microsoft.AspNetCore.Mvc;

namespace GastroWorld.Controllers
{
    public class TerminosController : Controller
    {
        public IActionResult Terminos()
        {
            return View("Terminos");
        }
    }
}
