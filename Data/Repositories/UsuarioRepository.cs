using Business.DTOs;
using Business.Interfaces.Usuario;
using Business.Models;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ContextDb db) : base(db)
        {
        }

        public async Task<List<UsuarioDto>> ListAsync()
        {
            return await Db.Usuario
                .Select(usuario => new UsuarioDto
                {
                    Nome = usuario.Nome,
                    Login = usuario.Login,
                    DataCadastro = usuario.DataCadastro,
                    DataAtualizacao = usuario.DataAtualizacao
                }).ToListAsync();
        }

        public async Task<Usuario> GetUsuarioById(long id)
        {
            return await Db.Usuario.FirstOrDefaultAsync(usuario => usuario.Id == id);
        }

        public async Task<Usuario> LoginExiste(string login)
        {
            return await Db.Usuario.FirstOrDefaultAsync(usuario => usuario.Login == login);
        }
    }
}