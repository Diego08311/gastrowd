using GastroWorld.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using GastroWorld.Models.NewFolder;
using GastroWorld.Models.IModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging; // Añade esta directiva

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
    [Route("api/Login")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUsuarioRepository usuarioRepository,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AuthController> logger)
        {
            _usuarioRepository = usuarioRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest model)
        {
            // Logging de intento de login
            _logger.LogInformation($"Intento de login: Usuario={model.Usuario}");

            try
            {
                var user = await _usuarioRepository.GetByCredential(model.Usuario, model.Password);

                if (user == null)
                {
                    _logger.LogWarning("Usuario no encontrado o credenciales inválidas");
                    return Unauthorized(new { message = "Credenciales inválidas" });
                }

                // Logging de usuario encontrado
                _logger.LogInformation($"Usuario encontrado: {user.usuario}, ID: {user.id_usuario}");

                // Establecer sesión
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    httpContext.Session.SetInt32("id_usuario", user.id_usuario);
                    _logger.LogInformation($"Sesión establecida para usuario: {user.id_usuario}");
                }
                else
                {
                    _logger.LogError("HttpContext es nulo. No se pudo establecer la sesión");
                }

                return Ok(new
                {
                    message = "Inicio de sesión exitoso",
                    user = new { user.id_usuario, user.usuario, user.email }
                });
            }
            catch (Exception ex)
            {
                // Logging de errores
                _logger.LogError(ex, $"Error durante el inicio de sesión para usuario {model.Usuario}");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
}