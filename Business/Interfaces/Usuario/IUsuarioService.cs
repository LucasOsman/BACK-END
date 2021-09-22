using Business.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.DTOs;

namespace Business.Interfaces.Usuario
{
    public interface IUsuarioService : IDisposable
    {
        Task<List<UsuarioDto>> ListAsync();
        Task PostUsuarioAsync(PostUsuarioRequest request);
        Task PutUsuarioAsync(PutUsuarioRequest request);
        Task DeleteUsuarioAsync(DeleteUsuarioRequest request);
    }
}