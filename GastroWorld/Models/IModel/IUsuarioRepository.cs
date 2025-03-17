using Gastroworld.Models.Model;
using GastroWorld.Repositories;

namespace GastroWorld.Models.IModel
{
    public interface IUsuarioRepository : IGenericRepository<Usuarios>
    {
        Task<Usuarios> GetByCredential(string usuario, string password);
        Task<bool> CreateUser(Usuarios usuario);
        Task<bool> UserExists(string usuario, string email);


    }
}
