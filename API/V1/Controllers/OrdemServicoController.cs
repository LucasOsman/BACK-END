using API.Controllers;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/OrdemServico")]
    public class OrdemServicoController : MainController
    {
        public OrdemServicoController(INotificador notificador) : base(notificador)
        {
        }
    }
}