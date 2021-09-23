using Business.DTOs;
using Business.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces.Usuario
{
    public interface IUsuarioService : IDisposable
    {
        Task<List<UsuarioDto>> ListAsync();
        Task PostUsuarioAsync(PostUsuarioRequest request);
        Task PutUsuarioAsync(PutUsuarioRequest request);
        Task DeleteUsuarioAsync(DeleteUsuarioRequest request);
        Task<UsuarioDto> GetByIdAsync(long id);
    }
}