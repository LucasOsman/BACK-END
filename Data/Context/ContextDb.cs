using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Context
{
    public class ContextDb : DbContext
    {
        private readonly IConfiguration _configuration;

        public ContextDb(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            ModelBuilderExtension.CreateModelBuilder(builder);

            base.OnModelCreating(builder);
        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Equipe> Equipe { get; set; }
        public DbSet<OrdemServico> OrdemServico { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}