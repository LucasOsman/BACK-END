using Business.Interfaces;
using Business.Interfaces.Cliente;
using Business.Models;
using Business.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Notificacoes;

namespace Service.Services
{
    public class ClienteService : BaseService, IClienteService
    {
        private readonly INotificador _notificador;
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(INotificador notificador, IClienteRepository clienteRepository) : base(notificador)
        {
            _notificador = notificador;
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

        public async Task PostClienteAsync(PostClienteRequest request)
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

        public async Task PutClienteAsync(PutClienteRequest request)
        {
            var cliente = await _clienteRepository.GetClienteById(request.Id);

            if (cliente == null)
            {
                _notificador.Handle(new Notificacao("Cliente não encontrado!"));
                return;
            }

            cliente.Nome = request.Nome;
            cliente.Email = request.Email;
            cliente.Telefone = request.Telefone;
            cliente.DataAtualizacao = DateTime.Now;

            await _clienteRepository.UpdateAsync(cliente);
        }

        public async Task DeleteClienteAsync(DeleteClienteRequest request)
        {
            var cliente = await _clienteRepository.GetClienteById(request.Id);

            if (cliente == null)
            {
                _notificador.Handle(new Notificacao("Cliente não encontrado!"));
                return;
            }

            await _clienteRepository.DeleteAsync(cliente);
        }
    }
}