using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces.OrdemServico
{
    public interface IOrdemServicoRepository : IRepository<Models.OrdemServico>
    {
        Task<List<Models.OrdemServico>> ListAsync();
        Task<Models.OrdemServico> GetOrdemServicoById(long idOrdemServico);
    }
}