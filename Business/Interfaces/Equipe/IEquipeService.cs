using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Requests;

namespace Business.Interfaces.Equipe
{
    public interface IEquipeService : IDisposable
    {
        Task<List<Models.Equipe>> ListAsync();
        Task PostOrdemServicoAsync(PostEquipeRequest request);
        Task PutEquipeAsync(PutEquipeRequest request);
        Task DeleteOrdemServicoAsync(DeleteEquipeRequest request);
    }
}