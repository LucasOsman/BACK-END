using Business.Interfaces;
using Business.Interfaces.Equipe;
using Business.Interfaces.OrdemServico;
using Business.Models;
using Business.Notificacoes;
using Business.Requests;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services
{
    public class OrdemServicoService : BaseService, IOrdemServicoService
    {
        private readonly IOrdemServicoRepository _ordemServicoRepository;
        private readonly IEquipeRepository _equipeRepository;
        private readonly INotificador _notificador;

        public OrdemServicoService(IOrdemServicoRepository ordemServicoRepository, IEquipeRepository equipeRepository, INotificador notificador) : base(notificador)
        {
            _ordemServicoRepository = ordemServicoRepository;
            _equipeRepository = equipeRepository;
            _notificador = notificador;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<List<OrdemServico>> ListAsync()
        {
            return await _ordemServicoRepository.ListAsync();
        }

        public async Task PostOrdemServicoAsync(PostOrdemServicoRequest request)
        {
            var equipeExiste = await _equipeRepository.GetEquipeById(request.IdEquipe);

            if (equipeExiste == null)
            {
                _notificador.Handle(new Notificacao("Equipe não encontrada!"));
                return;
            }

            var os = new OrdemServico
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                IdEquipe = request.IdEquipe,
                Status = request.Status,
                DataCadastro = DateTime.Now
            };

            await _ordemServicoRepository.CreateAsync(os);
        }

        public async Task PutOrdemServicoAsync(PutOrdemServicoRequest request)
        {
            var os = await _ordemServicoRepository.GetOrdemServicoById(request.IdOrdemServico);
            var equipeExiste = await _equipeRepository.EquipeExiste(request.IdEquipe);

            if (os == null)
            {
                _notificador.Handle(new Notificacao("Ordem de Serviço não encontrada!"));
                return;
            }

            if (equipeExiste == null)
            {
                _notificador.Handle(new Notificacao("Equipe não encontrada!"));
                return;
            }

            os.Nome = request.Nome;
            os.Descricao = request.Descricao;
            os.Status = request.Status;
            os.IdEquipe = request.IdEquipe;
            os.DataCadastro = DateTime.Now;

            await _ordemServicoRepository.UpdateAsync(os);
        }

        public async Task DeleteOrdemServicoAsync(DeleteOrdemServicoRequest request)
        {
            var os = await _ordemServicoRepository.GetOrdemServicoById(request.IdOrdemServico);

            if (os == null)
            {
                _notificador.Handle(new Notificacao("Ordem de Serviço não encontrada!"));
                return;
            }

            await _ordemServicoRepository.DeleteAsync(os);
        }
    }
}