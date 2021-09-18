using System.Threading.Tasks;
using API.Controllers;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.V1.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Cliente")]
    public class ClienteController : MainController
    {
        public ClienteController(INotificador notificador) : base(notificador)
        {
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            return CustomResponse();
        }
    }
}