using Microsoft.AspNetCore.Mvc;

namespace GastroWorld.Controllers
{
    public class PerfilEmpresaController : Controller
    {
        public IActionResult PerfilEmpresa()
        {
            return View("PerfilEmpresa");
        }
    }
}
