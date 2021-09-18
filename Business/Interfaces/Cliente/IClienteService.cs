using Business.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces.Cliente
{
    public interface IClienteService : IDisposable
    {
        Task<List<Models.Cliente>> ListAsync();
        Task PostClienteAsync(PostClienteRequest request);
        Task PutClienteAsync(PutClienteRequest request);
        Task DeleteClienteAsync(DeleteClienteRequest request);
    }
}