using Gastroworld.Data;
using Gastroworld.Models.Model;
using GastroWorld.Models;
using GastroWorld.Models.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GastroWorld.Controllers
{

    public class AjustesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AjustesController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Ajustes()
        {
            return View();
        }
        public IActionResult AjustesContext()
        {
            // Obtener el ID del usuario desde la sesión
            int? idUsuario = _httpContextAccessor.HttpContext.Session.GetInt32("id_usuario");

            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Login"); // Redirige si no hay sesión activa
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.id_usuario == idUsuario);
            return View(usuario);
        }

        [HttpPost]
        public IActionResult ActualizarUsuario(Usuarios usuario)
        {
            int? idUsuario = _httpContextAccessor.HttpContext.Session.GetInt32("id_usuario");

            if (idUsuario == null)
            {
                return RedirectToAction("Login", "Login"); // Redirige si no hay sesión activa
            }

            var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.id_usuario == idUsuario);
            if (usuarioExistente != null)
            {
                usuarioExistente.nombre = usuario.nombre;
                usuarioExistente.email = usuario.email;
                usuarioExistente.password = usuario.password;
                usuarioExistente.tipo_usuario = usuario.tipo_usuario;
                usuarioExistente.fecha_registro = usuario.fecha_registro;

                _context.SaveChanges();
                ViewBag.Mensaje = "Datos actualizados correctamente.";
            }
            else
            {
                ViewBag.Error = "Usuario no encontrado.";
            }

            return View("Ajustes", usuario);
        }
    }
}
