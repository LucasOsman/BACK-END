using API.Controllers;
using Business.Interfaces;
using Business.Interfaces.OrdemServico;
using Business.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/OrdemServico")]
    public class OrdemServicoController : MainController
    {
        private readonly IOrdemServicoService _ordemServicoService;


        public OrdemServicoController(INotificador notificador, IOrdemServicoService ordemServicoService) : base(notificador)
        {
            _ordemServicoService = ordemServicoService;
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var result = await _ordemServicoService.ListAsync();
            return CustomResponse(result);
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(PostOrdemServicoRequest request)
        {
            await _ordemServicoService.PostOrdemServicoAsync(request);
            return CustomResponse("Criado com sucesso!");
        }

        [HttpPut("Put")]
        public async Task<IActionResult> Put(PutOrdemServicoRequest request)
        {
            await _ordemServicoService.PutOrdemServicoAsync(request);
            return CustomResponse("Editado com sucesso!");
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(DeleteOrdemServicoRequest request)
        {
            await _ordemServicoService.DeleteOrdemServicoAsync(request);
            return CustomResponse("Deletado com sucesso!");
        }
    }
}