using Business.Interfaces;
using Business.Interfaces.Cliente;
using Business.Models;
using Business.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ClienteService : BaseService, IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(INotificador notificador, IClienteRepository clienteRepository) : base(notificador)
        {
            _clienteRepository = clienteRepository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<List<Cliente>> ListAsync()
        {
            return await _clienteRepository.ListAsync();
        }

        public async Task PostCliente(PostClienteRequest request)
        {
            var cliente = new Cliente
            {
                Nome = request.Nome,
                Email = request.Email,
                Telefone = request.Telefone,
                DataCadastro = DateTime.Now,
                DataAtualizacao = DateTime.Now
            };

            await _clienteRepository.CreateAsync(cliente);
        }
    }
}