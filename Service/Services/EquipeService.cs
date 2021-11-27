using Business.Interfaces;
using Business.Interfaces.Equipe;
using Business.Models;
using Business.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces.Usuario;
using Business.Notificacoes;

namespace Service.Services
{
    public class EquipeService : BaseService, IEquipeService
    {
        private readonly IEquipeRepository _equipeRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly INotificador _notificador;

        public EquipeService(INotificador notificador, IEquipeRepository equipeRepository, IUsuarioRepository usuarioRepository) : base(notificador)
        {
            _notificador = notificador;
            _equipeRepository = equipeRepository;
            _usuarioRepository = usuarioRepository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<List<Equipe>> ListAsync()
        {
            return await _equipeRepository.ListAsync();
        }

        public async Task PostOrdemServicoAsync(PostEquipeRequest request)
        {
            var usuarioExiste = await _usuarioRepository.GetUsuarioById(request.IdUsuario);

            if (usuarioExiste == null)
            {
                _notificador.Handle(new Notificacao("Usuário não encontrado!"));
                return;
            }

            var usuario = new Equipe
            {
                Nome = request.Nome,
                Observacao = request.Observacao,
                IdUsuario = request.IdUsuario,
                Excluido = false,
                DataCadastro = DateTime.Now,
                DataAtualizacao = DateTime.Now
            };

            await _equipeRepository.CreateAsync(usuario);
        }

        public async Task PutEquipeAsync(PutEquipeRequest request)
        {
            var equipe = await _equipeRepository.GetEquipeById(request.IdEquipe);
            var usuarioExiste = await _usuarioRepository.GetUsuarioById(request.IdUsuario);

            if (equipe == null)
            {
                _notificador.Handle(new Notificacao("Equipe não encontrada!"));
                return;
            }

            if (usuarioExiste == null)
            {
                _notificador.Handle(new Notificacao("Usuário não encontrado!"));
                return;
            }

            equipe.Nome = request.Nome;
            equipe.Observacao = request.Observacao;
            equipe.IdUsuario = request.IdUsuario;
            equipe.Excluido = false;
            equipe.DataAtualizacao = DateTime.Now;

            await _equipeRepository.UpdateAsync(equipe);
        }

        public async Task DeleteOrdemServicoAsync(DeleteEquipeRequest request)
        {
            var equipe = await _equipeRepository.GetEquipeById(request.IdEquipe);

            if (equipe == null)
            {
                _notificador.Handle(new Notificacao("Equipe não encontrada!"));
                return;
            }

            equipe.Excluido = true;

            await _equipeRepository.UpdateAsync(equipe);
        }
    }
}