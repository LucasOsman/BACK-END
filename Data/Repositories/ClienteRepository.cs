using Business.Interfaces.Cliente;
using Business.Models;
using Data.Context;

namespace Data.Repositories
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(ContextDb db) : base(db)
        {
        }
    }
}