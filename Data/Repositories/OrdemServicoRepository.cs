using Business.Interfaces.OrdemServico;
using Business.Models;
using Data.Context;

namespace Data.Repositories
{
    public class OrdemServicoRepository : Repository<OrdemServico>, IOrdemServicoRepository
    {
        public OrdemServicoRepository(ContextDb db) : base(db)
        {
        }
    }
}