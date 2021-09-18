using Business.Interfaces;
using Business.Interfaces.Cliente;
using Business.Interfaces.Equipe;
using Business.Interfaces.OrdemServico;
using Business.Interfaces.Usuario;
using Business.Notificacoes;
using Data.Context;
using Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Service.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<ContextDb>();
            services.AddScoped<INotificador, Notificador>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigOptions>();

            return services;
        }

        public static IServiceCollection ResolveRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IOrdemServicoRepository, OrdemServicoRepository>();
            services.AddScoped<IEquipeRepository, EquipeRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
         
            return services;
        }

        public static IServiceCollection ResolveServices(this IServiceCollection services)
        {
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IOrdemServicoService, OrdemServicoService>();
            services.AddScoped<IEquipeService, EquipeService>();
            services.AddScoped<IUsuarioService, UsuarioService>();

            return services;
        }
    }
}