using Business.Interfaces.Equipe;
using Business.Models;
using Data.Context;

namespace Data.Repositories
{
    public class EquipeRepository : Repository<Equipe>, IEquipeRepository
    {
        public EquipeRepository(ContextDb db) : base(db)
        {
        }
    }
}