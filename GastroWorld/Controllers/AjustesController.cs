using Microsoft.AspNetCore.Mvc;

namespace GastroWorld.Controllers
{
    public class AjustesController : Controller
    {
        public IActionResult Ajustes()
        {
            return View("Ajustes");
        }
    }
}
