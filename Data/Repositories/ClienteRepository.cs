using Business.Interfaces.Cliente;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(ContextDb db) : base(db)
        {
        }

        public async Task<List<Cliente>> ListAsync()
        {
            return await Db.Cliente.ToListAsync();
        }

        public async Task<Cliente> GetClienteById(long id)
        {
            return await Db.Cliente.FirstOrDefaultAsync(cliente => cliente.Id == id);
        }
    }
}