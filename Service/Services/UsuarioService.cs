using System;
using Business.Interfaces;
using Business.Interfaces.Usuario;

namespace Service.Services
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        public UsuarioService(INotificador notificador) : base(notificador)
        {
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}