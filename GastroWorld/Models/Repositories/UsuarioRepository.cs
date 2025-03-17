using Gastroworld.Data;
using GastroWorld.Models;
using Gastroworld.Models.Model;
using GastroWorld.Models.IModel;
using GastroWorld.Repositories;
using Microsoft.EntityFrameworkCore;


namespace GastroWorld.Models.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuarios>, IUsuarioRepository
    {

        private readonly ApplicationDbContext context;

        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Usuarios> GetByCredential(string usuario, string password)
        {
            var result = await context.Usuarios.FirstOrDefaultAsync(d => d.usuario == usuario && d.password == password);
            Console.WriteLine(result != null ? "Usuario encontrado" : "Usuario no encontrado");
            return result;
        }

        public async Task<bool> CreateUser(Usuarios usuario)
        {
            try
            {
                usuario.fecha_registro = DateTime.Now;
                await context.Usuarios.AddAsync(usuario);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UserExists(string usuario, string email)
        {
            return await context.Usuarios.AnyAsync(u => u.usuario == usuario || u.email == email);
        }

    }
}
