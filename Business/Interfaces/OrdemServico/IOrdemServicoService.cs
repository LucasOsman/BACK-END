using Business.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces.OrdemServico
{
    public interface IOrdemServicoService : IDisposable
    {
        Task<List<Models.OrdemServico>> ListAsync();
        Task PostOrdemServicoAsync(PostOrdemServicoRequest request);
        Task PutOrdemServicoAsync(PutOrdemServicoRequest request);
        Task DeleteOrdemServicoAsync(DeleteOrdemServicoRequest request);
    }
}