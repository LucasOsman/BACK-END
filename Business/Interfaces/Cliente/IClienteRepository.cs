using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces.Cliente
{
    public interface IClienteRepository : IRepository<Models.Cliente>
    {
        Task<List<Models.Cliente>> ListAsync();
    }
}