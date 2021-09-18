using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces.Usuario
{
    public interface IUsuarioRepository : IRepository<Models.Usuario>
    {
        Task<List<Models.Usuario>> ListAsync();
        Task<Models.Usuario> GetUsuarioById(long id);
        Task<Models.Usuario> LoginExiste(string login);
    }
}