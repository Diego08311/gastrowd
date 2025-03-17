using System.ComponentModel.DataAnnotations;

namespace GastroWorld.Models.Request
{
    public class UserRegisterRequest
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Usuario { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string TipoUsuario { get; set; }
    }
}



