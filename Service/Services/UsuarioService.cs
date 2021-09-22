using Business.Interfaces;
using Business.Interfaces.Usuario;
using Business.Models;
using Business.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.DTOs;
using Business.Notificacoes;

namespace Service.Services
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        private readonly INotificador _notificador;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(INotificador notificador, IUsuarioRepository usuarioRepository) : base(notificador)
        {
            _notificador = notificador;
            _usuarioRepository = usuarioRepository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<List<UsuarioDto>> ListAsync()
        {
            return await _usuarioRepository.ListAsync();
        }

        public async Task PostUsuarioAsync(PostUsuarioRequest request)
        {
            var loginExiste = await _usuarioRepository.LoginExiste(request.Login);

            if (loginExiste != null)
            {
                _notificador.Handle(new Notificacao("Login não está disponível para uso!"));
                return;
            }

            var usuario = new Usuario
            {
                Login = request.Login,
                Nome = request.Nome,
                Senha = request.Senha,
                DataCadastro = DateTime.Now,
                DataAtualizacao = DateTime.Now
            };

            await _usuarioRepository.CreateAsync(usuario);
        }

        public async Task PutUsuarioAsync(PutUsuarioRequest request)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(request.Id);

            if (usuario == null)
            {
                _notificador.Handle(new Notificacao("Usuário não encontrado!"));
                return;
            }

            usuario.Nome = request.Nome;
            usuario.Senha = request.Senha;
            usuario.DataAtualizacao = DateTime.Now;

            await _usuarioRepository.UpdateAsync(usuario);
        }

        public async Task DeleteUsuarioAsync(DeleteUsuarioRequest request)
        {
            var usuario = await _usuarioRepository.GetUsuarioById(request.Id);

            if (usuario == null)
            {
                _notificador.Handle(new Notificacao("Usuário não encontrado!"));
                return;
            }

            await _usuarioRepository.DeleteAsync(usuario);
        }
    }
}