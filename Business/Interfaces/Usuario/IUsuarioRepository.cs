using Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces.Usuario
{
    public interface IUsuarioRepository : IRepository<Models.Usuario>
    {
        Task<List<UsuarioDto>> ListAsync();
        Task<UsuarioDto> GetByIdForUpdate(long id);
        Task<Models.Usuario> GetUsuarioById(long id);
        Task<Models.Usuario> LoginExiste(string login);
    }
}