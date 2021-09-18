using Business.Interfaces;
using Business.Interfaces.OrdemServico;
using System;

namespace Service.Services
{
    public class OrdemServicoService : BaseService, IOrdemServicoService
    {

        public OrdemServicoService(INotificador notificador) : base(notificador)
        {
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}