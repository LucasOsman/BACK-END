using Business.Interfaces.OrdemServico;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class OrdemServicoRepository : Repository<OrdemServico>, IOrdemServicoRepository
    {
        public OrdemServicoRepository(ContextDb db) : base(db)
        {
        }

        public async Task<List<OrdemServico>> ListAsync()
        {
            return await Db.OrdemServico.ToListAsync();
        }

        public async Task<OrdemServico> GetOrdemServicoById(long idOrdemServico)
        {
            return await Db.OrdemServico.FirstOrDefaultAsync(os => os.Id == idOrdemServico);
        }
    }
}