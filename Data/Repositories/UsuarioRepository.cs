using Business.Interfaces.Usuario;
using Business.Models;
using Data.Context;

namespace Data.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ContextDb db) : base(db)
        {
        }
    }
}