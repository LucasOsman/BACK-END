//using API.Controllers;
//using Business.Filters.Questionario;
//using Business.Interfaces;
//using Business.Interfaces.IQuestionario;
//using Business.Interfaces.IQuestionario.Export;
//using Business.Requests.Questionario;
//using Business.Validations.Questionario;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;

//namespace API.V1.Controllers
//{
//    [Authorize]
//    [ApiVersion("1.0")]
//    [Route("api/v{version:apiVersion}/Checklist")]
//    public class QuestionarioController : MainController
//    {
//        private readonly IQuestionarioService _questionarioService;
//        private readonly IQuestionarioPadraoExport _questionarioPadraoExport;
//        private readonly IQuestionarioColunaExport _questionarioColunaExport;
//        private readonly IQuestionarioEstatisticaExport _questionarioEstatisticaExport;

//        public QuestionarioController(INotificador notificador,
//            IUser user,
//            IQuestionarioService questionarioService,
//            IQuestionarioPadraoExport questionarioPadraoExport,
//            IQuestionarioColunaExport questionarioColunaExport,
//            IQuestionarioEstatisticaExport questionarioEstatisticaExport) : base(notificador, user)
//        {
//            _questionarioService = questionarioService;
//            _questionarioPadraoExport = questionarioPadraoExport;
//            _questionarioColunaExport = questionarioColunaExport;
//            _questionarioEstatisticaExport = questionarioEstatisticaExport;
//        }

//        [HttpPost("List")]
//        [ApiExplorerSettings(IgnoreApi = true)]
//        public async Task<IActionResult> List(QuestionarioFilter filter)
//        {
//            var validator = new QuestionarioValidation();

//            var requestValido = await ValidationRequestAsync(validator, filter);

//            if (!requestValido)
//                return CustomResponse();

//            var listQuestionarioAsync = await _questionarioService.ListAsync(filter);
//            return CustomResponse(listQuestionarioAsync);
//        }

//        [HttpPost("Get")]
//        public async Task<IActionResult> Get(QuestionarioGetFilter filter)
//        {
//            var getQuestionario = await _questionarioService.Get(filter);

//            return CustomResponse(getQuestionario);
//        }

//        [HttpGet("ListCriadoPor")]
//        [ApiExplorerSettings(IgnoreApi = true)]
//        public async Task<IActionResult> ListCriadoPor()
//        {
//            var listCriadoPorAsync = await _questionarioService.ListCriadoPorAsync();

//            return CustomResponse(listCriadoPorAsync);
//        }

//        [HttpPost("ListSegmento")]
//        public async Task<IActionResult> ListSegmento(ListSegmentoFilter filter)
//        {
//            var listSegmentoAsync = await _questionarioService.ListSegmentoAsync(filter);

//            return CustomResponse(listSegmentoAsync);
//        }

//        [HttpPost("ListEquipe")]
//        public async Task<IActionResult> ListEquipe(ListEquipeFilter filter)
//        {
//            var listEquipeAsync = await _questionarioService.ListEquipeAsync(filter);

//            return CustomResponse(listEquipeAsync);
//        }

//        [HttpPost("Post")]
//        public async Task<IActionResult> Post(QuestionarioPostRequest request)
//        {
//            var validator = new QuestionarioPostValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            var questionario = await _questionarioService.Post(request);
//            return CustomResponse(questionario);
//        }

//        [HttpPut("Put")]
//        public async Task<IActionResult> Put(QuestionarioPutRequest request)
//        {
//            var validator = new QuestionarioPutValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            await _questionarioService.Put(request);
//            return CustomResponse();
//        }

//        [HttpPost("PostSecao")]
//        public async Task<IActionResult> PostSecao(QuestionarioPostSecaoRequest request)
//        {
//            var validator = new QuestionarioPostSecaoValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            var questionarioSecao = await _questionarioService.PostSecao(request);
//            return CustomResponse(questionarioSecao);
//        }

//        [HttpPut("PutSecao")]
//        public async Task<IActionResult> PutSecao(SecaoQuestionarioPutRequest request)
//        {
//            var validator = new SecaoQuestionarioPutValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            await _questionarioService.PutSecao(request);
//            return CustomResponse();
//        }

//        [HttpPost("GetSecao")]
//        public async Task<IActionResult> GetSecao(QuestionarioGetSecaoFilter filter)
//        {
//            var getSecao = await _questionarioService.GetSecao(filter);

//            return CustomResponse(getSecao);
//        }

//        [HttpPost("PostQuestao")]
//        public async Task<IActionResult> PostQuestao(QuestionarioPostQuestaoRequest request)
//        {
//            var validator = new QuestionarioPostQuestaoValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            var questionarioQuestao = await _questionarioService.PostQuestao(request);
//            return CustomResponse(questionarioQuestao);
//        }

//        [HttpPut("PutQuestao")]
//        public async Task<IActionResult> PutQuestao(QuestaoQuestionarioPutRequest request)
//        {
//            var validator = new QuestaoQuestionarioPutValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            await _questionarioService.PutQuestao(request);
//            return CustomResponse();
//        }

//        [HttpPost("GetQuestao")]
//        public async Task<IActionResult> GetQuestao(QuestaoGetFilter filter)
//        {
//            var getSecao = await _questionarioService.GetQuestao(filter);

//            return CustomResponse(getSecao);
//        }

//        [HttpPost("VincularQuestionarioSegmento")]
//        public async Task<IActionResult> VincularQuestionarioSegmento(VincularQuestionarioSegmentoRequest request)
//        {
//            var validator = new VincularQuestionarioSegmentoValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            await _questionarioService.VincularQuestionarioSegmento(request);
//            return CustomResponse();
//        }

//        [HttpPost("VincularQuestionarioGrupo")]
//        public async Task<IActionResult> VincularQuestionarioGrupo(VincularQuestionarioGrupoRequest request)
//        {
//            var validator = new VincularQuestionarioGrupoValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            await _questionarioService.VincularQuestionarioGrupo(request);
//            return CustomResponse();
//        }

//        [HttpPost("PostCabecalho/{idQuestionario:int:min(1)}")]
//        public async Task<IActionResult> PostCabecalho(long idQuestionario)
//        {
//            var result = await _questionarioService.PostCabecalho(idQuestionario, Request.Form.Files);
//            return Ok(result);
//        }

//        [HttpPost("PostRodape/{idQuestionario:int:min(1)}")]
//        public async Task<IActionResult> PostRodape(long idQuestionario)
//        {
//            var result = await _questionarioService.PostRodape(idQuestionario, Request.Form.Files);
//            return Ok(result);
//        }

//        [HttpPost("DeleteCabecalho")]
//        public async Task<IActionResult> DeleteCabecalho(DeleteCabecalhoRodapeRequest request)
//        {
//            await _questionarioService.DeleteCabecalho(request.IdQuestionario);
//            return Ok();
//        }

//        [HttpPost("DeleteRodape")]
//        public async Task<IActionResult> DeleteRodape(DeleteCabecalhoRodapeRequest request)
//        {
//            await _questionarioService.DeleteRodape(request.IdQuestionario);
//            return Ok();
//        }

//        [HttpPost("ExcluirSecao")]
//        public async Task<IActionResult> ExcluirSecao(ExcluirSecaoQuestionarioRequest request)
//        {
//            await _questionarioService.ExcluirSecao(request);
//            return CustomResponse();
//        }

//        [HttpPost("ExcluirQuestao")]
//        public async Task<IActionResult> ExcluirQuestao(ExcluirQuestaoQuestionarioRequest request)
//        {
//            await _questionarioService.ExcluirQuestao(request);
//            return CustomResponse();
//        }

//        [HttpPost("ListQuestao")]
//        public async Task<IActionResult> ListQuestao(ListQuestaoQuestionarioRequest request)
//        {
//            var validator = new ListQuestaoQuestionarioValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            var listQuestao = await _questionarioService.ListQuestao(request);
//            return Ok(listQuestao);
//        }

//        [HttpPost("GetVinculosSegmento")]
//        public async Task<IActionResult> GetVinculosSegmento(GetVinculosSegmentoRequest request)
//        {
//            var validator = new GetVinculosSegmentoValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            var listVinculoSegmento = await _questionarioService.GetVinculosSegmento(request);
//            return CustomResponse(listVinculoSegmento);
//        }

//        [HttpPost("GetVinculosEquipe")]
//        public async Task<IActionResult> GetVinculosEquipe(GetVinculosEquipeRequest request)
//        {
//            var validator = new GetVinculosEquipeValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            var listVinculoEquipe = await _questionarioService.GetVinculosEquipe(request);
//            return CustomResponse(listVinculoEquipe);
//        }

//        [HttpPost("ListItemQuestao")]
//        public async Task<IActionResult> ListItemQuestao(ListItemQuestaoRequest request)
//        {
//            var validator = new ListItemQuestaoValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            var listVinculoEquipe = await _questionarioService.ListItemQuestao(request);
//            return CustomResponse(listVinculoEquipe);
//        }

//        [HttpPost("ListSecao")]
//        public async Task<IActionResult> ListSecao(ListSecaoRequest request)
//        {
//            var validator = new ListSecaoValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            var listSecao = await _questionarioService.ListSecao(request);
//            return CustomResponse(listSecao);
//        }

//        [HttpPost("PostItemQuestao")]
//        public async Task<IActionResult> PostItemQuestao(PostItemQuestaoRequest request)
//        {
//            var validator = new PostItemQuestaoValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            await _questionarioService.PostItemQuestao(request);
//            return CustomResponse();
//        }

//        [HttpPut("PutItemQuestao")]
//        public async Task<IActionResult> PutItemQuestao(PutItemQuestaoRequest request)
//        {
//            var validator = new PutItemQuestaoValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            await _questionarioService.PutItemQuestao(request);
//            return CustomResponse();
//        }

//        [HttpPost("GetItemQuestao")]
//        public async Task<IActionResult> GetItemQuestao(GetItemQuestaoFilter filter)
//        {
//            var getItemQuestao = await _questionarioService.GetItemQuestao(filter);

//            return CustomResponse(getItemQuestao);
//        }

//        [HttpPost("ExcluirItemQuestao")]
//        public async Task<IActionResult> ExcluirItemQuestao(ExcluirItemQuestaoRequest request)
//        {
//            var validator = new ExcluirItemQuestaoValidation();

//            var requestValido = await ValidationRequestAsync(validator, request);

//            if (!requestValido)
//                return CustomResponse();

//            await _questionarioService.ExcluirItemQuestao(request);
//            return CustomResponse();
//        }

//        [HttpPost("Ativar")]
//        [ApiExplorerSettings(IgnoreApi = true)]
//        public async Task<IActionResult> Ativar(AtivarQuestionarioFilter filter)
//        {
//            var validator = new AtivarQuestionarioValidation();

//            var requestValido = await ValidationRequestAsync(validator, filter);

//            if (!requestValido)
//                return CustomResponse();

//            await _questionarioService.AtivarQuestionario(filter);
//            return CustomResponse();
//        }

//        [HttpPost("Desativar")]
//        [ApiExplorerSettings(IgnoreApi = true)]
//        public async Task<IActionResult> Desativar(DesativarQuestionarioFilter filter)
//        {
//            var validator = new DesativarQuestionarioValidation();

//            var requestValido = await ValidationRequestAsync(validator, filter);

//            if (!requestValido)
//                return CustomResponse();

//            await _questionarioService.DesativarQuestionario(filter);
//            return CustomResponse();
//        }

//        [HttpPost("ExportarQuestionarioPadrao")]
//        public async Task<IActionResult> ExportarQuestionarioPadrao(ExportarQuestionarioPadraoFilter filter)
//        {
//            var validator = new ExportarQuestionarioPadraoValidation();

//            var requestValido = await ValidationRequestAsync(validator, filter);

//            if (!requestValido)
//                return CustomResponse();

//            var excelResult = await _questionarioPadraoExport.PadraoAsync(filter.IdsQuestionarios);

//            return Ok(excelResult);
//        }

//        [HttpPost("ExportarQuestionarioPorColuna")]
//        public async Task<IActionResult> ExportarQuestionarioPorColuna(ExportQuestionarioColunaFilter filter)
//        {
//            var validator = new ExportarQuestionarioColunaValidation();

//            var requestValido = await ValidationRequestAsync(validator, filter);

//            if (!requestValido)
//                return CustomResponse();

//            var excelResult = await _questionarioColunaExport.ColunaAsync(filter.IdsQuestionarios);

//            return Ok(excelResult);
//        }

//        [HttpPost("ExportarQuestionarioEstatistica")]
//        public async Task<IActionResult> ExportarQuestionarioEstatistica(ExportQuestionarioEstatisticaFilter filter)
//        {
//            var validator = new ExportarQuestionarioEstatisticaValidation();

//            var requestValido = await ValidationRequestAsync(validator, filter);

//            if (!requestValido)
//                return CustomResponse();

//            var excelResult = await _questionarioEstatisticaExport.EstatisticaAsync(filter.IdsQuestionarios);

//            return Ok(excelResult);
//        }
//    }
//}