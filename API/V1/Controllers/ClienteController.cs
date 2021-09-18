using API.Controllers;
using Business.Interfaces;
using Business.Interfaces.Cliente;
using Business.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Cliente")]
    public class ClienteController : MainController
    {
        private readonly IClienteService _clienteService;

        public ClienteController(INotificador notificador, IClienteService clienteService) : base(notificador)
        {
            _clienteService = clienteService;
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var result = await _clienteService.ListAsync();
            return CustomResponse(result);
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(PostClienteRequest request)
        {
            await _clienteService.PostClienteAsync(request);
            return CustomResponse("Criado com sucesso!");
        }

        [HttpPut("Put")]
        public async Task<IActionResult> Put(PutClienteRequest request)
        {
            await _clienteService.PutClienteAsync(request);
            return CustomResponse("Editado com sucesso!");
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(DeleteClienteRequest request)
        {
            await _clienteService.DeleteClienteAsync(request);
            return CustomResponse("Deletado com sucesso!");
        }
    }
}