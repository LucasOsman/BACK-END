using API.Controllers;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Equipe")]
    public class EquipeController : MainController
    {
        public EquipeController(INotificador notificador) : base(notificador)
        {
        }
    }
}