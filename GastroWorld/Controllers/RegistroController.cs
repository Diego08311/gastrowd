using Gastroworld.Models.Model;
using GastroWorld.Models;
using GastroWorld.Models.IModel;
using GastroWorld.Models.Request;
using GastroWorld.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;



namespace GastroWorld.Controllers
{
    public class RegistroController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public RegistroController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        [Route("api/auth/register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Datos de registro incompletos" });
            }

            // Verificar si el usuario o email ya existe
            var userExists = await _usuarioRepository.UserExists(model.Usuario, model.Email);
            if (userExists)
            {
                return BadRequest(new { message = "El nombre de usuario o email ya está en uso" });
            }

            // Crear el nuevo usuario
            var newUser = new Usuarios
            {
                nombre = model.Nombre,
                usuario = model.Usuario,
                email = model.Email,
                password = model.Password, 
                tipo_usuario = model.TipoUsuario,
                fecha_registro = System.DateTime.Now
            };

            var result = await _usuarioRepository.CreateUser(newUser);
            if (result)
            {
                return Ok(new { message = "Registro exitoso", user = new { id_usuario = newUser.id_usuario, usuario = newUser.usuario, email = newUser.email } });
            }
            else
            {
                return StatusCode(500, new { message = "Error al registrar usuario" });
            }
        }
    }
}
