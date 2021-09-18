using Business.Interfaces;
using Business.Notificacoes;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MoreLinq.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotificador _notificador;

        protected MainController(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
            });
        }

        protected void NotificarErro(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }

        protected async Task<bool> ValidationRequestAsync<TValidator, TFilter>(TValidator validator, TFilter filter) where TValidator : AbstractValidator<TFilter>
        {
            var result = await validator.ValidateAsync(filter);

            if (!result.IsValid)
                result.Errors.ForEach(x => NotificarErro(x.ErrorMessage));

            return OperacaoValida();
        }
    }
}