using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions<ContextDb> options) : base(options) { }
        
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Equipe> Equipe { get; set; }
        public DbSet<OrdemServico> OrdemServico { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}