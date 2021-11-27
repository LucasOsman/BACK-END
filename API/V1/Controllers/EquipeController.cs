using API.Controllers;
using Business.Interfaces;
using Business.Interfaces.Equipe;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Business.Requests;

namespace API.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Equipe")]
    public class EquipeController : MainController
    {
        private readonly IEquipeService _equipeService;

        public EquipeController(INotificador notificador, IEquipeService equipeService) : base(notificador)
        {
            _equipeService = equipeService;
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var result = await _equipeService.ListAsync();
            return CustomResponse(result);
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(PostEquipeRequest request)
        {
            await _equipeService.PostOrdemServicoAsync(request);
            return CustomResponse("Criado com sucesso!");
        }

        [HttpPut("Put")]
        public async Task<IActionResult> Put(PutEquipeRequest request)
        {
            await _equipeService.PutEquipeAsync(request);
            return CustomResponse("Editado com sucesso!");
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(DeleteEquipeRequest request)
        {
            await _equipeService.DeleteOrdemServicoAsync(request);
            return CustomResponse("Deletado com sucesso!");
        }
    }
}