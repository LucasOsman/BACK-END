using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces.Equipe
{
    public interface IEquipeRepository : IRepository<Models.Equipe>
    {
        Task<Models.Equipe> GetEquipeById(long idEquipe);
        Task<List<Models.Equipe>> ListAsync();
    }
}