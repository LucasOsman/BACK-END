using Business.Interfaces.Equipe;
using System;
using Business.Interfaces;

namespace Service.Services
{
    public class EquipeService : BaseService, IEquipeService
    {

        public EquipeService(INotificador notificador) : base(notificador)
        {
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}