using Business.Interfaces.Equipe;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class EquipeRepository : Repository<Equipe>, IEquipeRepository
    {
        public EquipeRepository(ContextDb db) : base(db)
        {
        }

        public async Task<Equipe> GetEquipeById(long idEquipe)
        {
            return await Db.Equipe.FirstOrDefaultAsync(equipe => equipe.Id == idEquipe);
        }

        public async Task<List<Equipe>> ListAsync()
        {
            return await Db.Equipe.ToListAsync();
        }
    }
}