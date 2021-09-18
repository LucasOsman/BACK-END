using API.Controllers;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Usuario")]
    public class UsuarioController : MainController
    {
        public UsuarioController(INotificador notificador) : base(notificador)
        {
        }
    }
}