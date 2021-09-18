using System;
using Business.Interfaces;
using Business.Interfaces.Cliente;

namespace Service.Services
{
    public class ClienteService : BaseService, IClienteService
    {
        public ClienteService(INotificador notificador) : base(notificador)
        {
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}