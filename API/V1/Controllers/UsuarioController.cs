using API.Controllers;
using Business.Interfaces;
using Business.Interfaces.Usuario;
using Business.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Usuario")]
    public class UsuarioController : MainController
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(INotificador notificador, IUsuarioService usuarioService) : base(notificador)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            var result = await _usuarioService.ListAsync();
            return CustomResponse(result);
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(PostUsuarioRequest request)
        {
            await _usuarioService.PostUsuarioAsync(request);
            return CustomResponse("Criado com sucesso!");
        }

        [HttpPut("Put")]
        public async Task<IActionResult> Put(PutUsuarioRequest request)
        {
            await _usuarioService.PutUsuarioAsync(request);
            return CustomResponse("Editado com sucesso!");
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(DeleteUsuarioRequest request)
        {
            await _usuarioService.DeleteUsuarioAsync(request);
            return CustomResponse("Deletado com sucesso!");
        }
    }
}