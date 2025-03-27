using GastroWorld.Models.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using GastroWorld.Models.NewFolder;
using GastroWorld.Models.IModel;
using Microsoft.AspNetCore.Http;


namespace GastroWorld.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View("Login");
        }
    }

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IUsuarioRepository usuarioRepository, IHttpContextAccessor httpContextAccessor) 
        {
            _usuarioRepository = usuarioRepository;
            _httpContextAccessor = httpContextAccessor;

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest model)
        {
            Console.WriteLine($"Intento de login: Usuario={model.Usuario}, Password={model.Password}");
            var user = await _usuarioRepository.GetByCredential(model.Usuario, model.Password);

            if (user == null)
            {
                Console.WriteLine("Usuario no encontrado o credenciales inválidas.");
                return Unauthorized(new { message = "Credenciales inválidas" });
            }
            Console.WriteLine($"Usuario encontrado: {user.usuario}");

            _httpContextAccessor.HttpContext.Session.SetInt32("id_usuario", user.id_usuario);

            return Ok(new
            {
                message = "Inicio de sesión exitoso",
                
                user = new { user.id_usuario, user.usuario, user.email }
            });
        }
    }


}

