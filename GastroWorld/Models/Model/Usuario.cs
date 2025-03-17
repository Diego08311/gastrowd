using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GastroWorld.Models.Model;

namespace Gastroworld.Models.Model
{
    public class Usuarios
    {
        [Key]
        public int id_usuario { get; set; }

        [Required, MaxLength(100)]
        public string nombre { get; set; }

        [Required, MaxLength(100)]
        public string usuario {  get; set; }

        [Required, MaxLength(150)]
        public string email { get; set; }

        [Required, MaxLength(255)]
        public string password { get; set; }

        [Required, MaxLength(100)]
        public string  tipo_usuario { get; set; }

        //[Required]
        //public string Foto { get; set; }

        [Required]
        public DateTime fecha_registro { get; set; } = DateTime.UtcNow;

        public ICollection<Receta> Recetas { get; set; }
        public ICollection<Publicacion> Publicaciones { get; set; }
    }
}