using System.Collections.Generic;
using System.Threading.Tasks;
using Business.DTOs;

namespace Business.Interfaces.Usuario
{
    public interface IUsuarioRepository : IRepository<Models.Usuario>
    {
        Task<List<UsuarioDto>> ListAsync();
        Task<Models.Usuario> GetUsuarioById(long id);
        Task<Models.Usuario> LoginExiste(string login);
    }
}