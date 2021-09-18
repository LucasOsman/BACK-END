//using AutoMapper;
//using Business.DTOs.V1.Equipe;
//using Business.DTOs.V1.Questionario;
//using Business.Extensions;
//using Business.Filters.Questionario;
//using Business.Interfaces;
//using Business.Interfaces.IAWS;
//using Business.Interfaces.IItemQuestao;
//using Business.Interfaces.IQuestao;
//using Business.Interfaces.IQuestionario;
//using Business.Interfaces.IQuestionarioGrupo;
//using Business.Interfaces.IQuestionarioSegmentoService;
//using Business.Interfaces.ISecaoQuestionario;
//using Business.Interfaces.ISegmento;
//using Business.Models;
//using Business.Notificacoes;
//using Business.Requests.Questionario;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Options;
//using Shared.Enums;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Shared.Utils;

//namespace Service.Services
//{
//    public class QuestionarioService : IQuestionarioService
//    {
//        private readonly IQuestionarioRepository _questionarioRepository;
//        private readonly ISegmentoRepository _segmentoRepository;
//        private readonly ISecaoQuestionarioRepository _secaoQuestionarioRepository;
//        private readonly IQuestaoRepository _questaoRepository;
//        private readonly IQuestionarioSegmentoRepository _questionarioSegmentoRepository;
//        private readonly IQuestionarioGrupoRepository _questionarioGrupoRepository;
//        private readonly IItemQuestaoRepository _itemQuestaoRepository;
//        private readonly IAwsService _awsService;
//        private readonly IUser _user;
//        private readonly IMapper _mapper;
//        private readonly INotificador _notificador;
//        private readonly S3Extension _s3Extension;

//        public QuestionarioService(IQuestionarioRepository questionarioRepository,
//            INotificador notificador,
//            ISegmentoRepository segmentoRepository,
//            IMapper mapper,
//            IUser user,
//            ISecaoQuestionarioRepository secaoQuestionarioRepository,
//            IQuestaoRepository questaoRepository,
//            IQuestionarioSegmentoRepository questionarioSegmentoRepository,
//            IQuestionarioGrupoRepository questionarioGrupoRepository,
//            IAwsService awsService,
//            IOptions<S3Extension> s3Options,
//            IItemQuestaoRepository itemQuestaoRepository)
//        {
//            _questionarioRepository = questionarioRepository;
//            _notificador = notificador;
//            _segmentoRepository = segmentoRepository;
//            _mapper = mapper;
//            _user = user;
//            _secaoQuestionarioRepository = secaoQuestionarioRepository;
//            _questaoRepository = questaoRepository;
//            _questionarioSegmentoRepository = questionarioSegmentoRepository;
//            _questionarioGrupoRepository = questionarioGrupoRepository;
//            _awsService = awsService;
//            _itemQuestaoRepository = itemQuestaoRepository;
//            _s3Extension = s3Options.Value;
//        }

//        public void Dispose()
//        {
//            GC.SuppressFinalize(this);
//        }

//        public async Task<List<QuestionarioDto>> ListAsync(QuestionarioFilter filter)
//        {
//            return await _questionarioRepository.ListAsync(filter);
//        }

//        public async Task<QuestionarioDto> Get(QuestionarioGetFilter filter)
//        {
//            var questionario = await _questionarioRepository.GetQuestionarioByIdListAsync(filter.IdQuestionario);

//            if (questionario == null)
//            {
//                _notificador.Handle(new Notificacao("O idQuestionario passado não existe."));
//                return null;
//            }

//            var questionarioMapper = _mapper.Map<QuestionarioDto>(questionario);
//            questionarioMapper.InicioVigencia = TimeZoneInfo.ConvertTimeFromUtc(questionario.InicioVigencia, TimeZoneUtils.GeTimeZoneBrasilia());
//            questionarioMapper.FimVigencia = TimeZoneInfo.ConvertTimeFromUtc((DateTime)questionario.FimVigencia, TimeZoneUtils.GeTimeZoneBrasilia());

//            if (!string.IsNullOrEmpty(questionario.Rodape))
//                questionarioMapper.Rodape = $"{_s3Extension.UrlS3}{questionarioMapper.Rodape}";

//            if (!string.IsNullOrEmpty(questionario.Cabecalho))
//                questionarioMapper.Cabecalho = $"{_s3Extension.UrlS3}{questionarioMapper.Cabecalho}";

//            return questionarioMapper;
//        }

//        public async Task<List<QuestionarioListCriadoPorDto>> ListCriadoPorAsync()
//        {
//            return await _questionarioRepository.QuestionarioListCriadoPorAsync();
//        }

//        public async Task<List<QuestionarioListSegmentoDto>> ListSegmentoAsync(ListSegmentoFilter filter)
//        {
//            return await _segmentoRepository.QuestionarioListSegmentoAsync(filter);
//        }

//        public async Task<List<EquipeDto>> ListEquipeAsync(ListEquipeFilter filter)
//        {
//            return await _questionarioRepository.ListEquipeAsync(filter);
//        }

//        public async Task AtivarQuestionario(AtivarQuestionarioFilter filter)
//        {
//            var questionarios = await _questionarioRepository.GetQuestionarioByIdsAsync(filter.IdsQuestionario, false);

//            var questionarioList = questionarios.ToList();

//            if (NenhumQuestionarioEncontrado(questionarioList, filter.IdsQuestionario))
//                return;

//            foreach (var questionario in questionarioList)
//            {
//                if (QuestionarioCompletoNoPrazo(questionario))
//                    return;

//                questionario.IdStatus = (int)EStatusQuestionario.Publicado;
//                questionario.EdicaoCorpoBloqueada = true;
//                questionario.Ativo = true;
//                questionario.DataAtualizacao = DateTime.UtcNow;
//            }

//            await _questionarioRepository.UpdateArrayAsync(questionarioList);
//        }

//        public async Task DesativarQuestionario(DesativarQuestionarioFilter filter)
//        {
//            var questionarios = await _questionarioRepository.GetQuestionarioByIdsAsync(filter.IdsQuestionario);

//            var questionarioList = questionarios.ToList();

//            if (NenhumQuestionarioEncontrado(questionarioList, filter.IdsQuestionario))
//                return;

//            foreach (var questionario in questionarioList)
//            {
//                if (QuestionarioIncompleto(questionario))
//                    return;

//                questionario.IdStatus = (int)EStatusQuestionario.NaoPublicado;
//                questionario.Ativo = false;
//                questionario.DataAtualizacao = DateTime.UtcNow;
//            }

//            await _questionarioRepository.UpdateArrayAsync(questionarioList);
//        }

//        public async Task<QuestionarioDto> Post(QuestionarioPostRequest request)
//        {
//            var createQuestionario = CreateQuestionarioModel(request);
//            var nomeExistente = await _questionarioRepository.GetQuestionarioByNomeAsync(request.Nome);

//            if (nomeExistente)
//            {
//                _notificador.Handle(new Notificacao($"Já existe um questionário com esse nome: {request.Nome}"));
//                return null;
//            }

//            await _questionarioRepository.CreateAsync(createQuestionario);

//            var mapperQuestionario = _mapper.Map<QuestionarioDto>(createQuestionario);
//            createQuestionario.Id = mapperQuestionario.Id;

//            return mapperQuestionario;
//        }

//        public async Task Put(QuestionarioPutRequest request)
//        {
//            var questionario = await _questionarioRepository.GetQuestionarioByIdAsync(request.Id);
//            var nomeExistente = await _questionarioRepository.GetQuestionarioByNomeAndIdQuestionarioAsync(request.Nome, request.Id);

//            if (nomeExistente)
//            {
//                _notificador.Handle(new Notificacao($"Já existe um questionário com esse nome: {request.Nome}"));
//                return;
//            }

//            if (ValidacaoQuestionarioEncontrado(request.Id, questionario))
//                return;

//            if (VerificaFimVigenciaNull(request.FimVigencia))
//                request.FimVigencia = DateTime.UtcNow.AddYears(100);

//            questionario.Nome = request.Nome;
//            questionario.Descricao = request.Descricao;
//            questionario.NumeroInicial = request.NumeroInicial;
//            questionario.InicioVigencia = request.InicioVigencia;
//            questionario.FimVigencia = request.FimVigencia;
//            questionario.Obrigatorio = request.Obrigatorio;
//            questionario.EnviarQuestionarioEmail = request.EnviarQuestionarioEmail;
//            questionario.ConexaoInternetObrigatoria = request.ConexaoInternetObrigatoria;
//            questionario.ExibirNovoRegistro = request.ExibirNovoRegistro;
//            questionario.DataAtualizacao = DateTime.UtcNow;

//            await _questionarioRepository.UpdateAsync(questionario);
//        }

//        public async Task<SecaoQuestionarioDto> PostSecao(QuestionarioPostSecaoRequest request)
//        {
//            var createSecao = await CreateSecaoModel(request);

//            await _secaoQuestionarioRepository.CreateAsync(createSecao);

//            var mapperSecaoQuestionario = _mapper.Map<SecaoQuestionarioDto>(createSecao);

//            mapperSecaoQuestionario.IdSecaoQuestionario = createSecao.Id;

//            return mapperSecaoQuestionario;
//        }

//        public async Task PutSecao(SecaoQuestionarioPutRequest request)
//        {
//            var secaoQuestionario = await _secaoQuestionarioRepository.GetSecaoQuestionarioById(request.IdSecaoQuestionario);

//            if (secaoQuestionario == null)
//            {
//                _notificador.Handle(new Notificacao($"Nenhuma seção encontrada com este id: {request.IdSecaoQuestionario}"));
//                return;
//            }

//            secaoQuestionario.Nome = request.Nome;
//            secaoQuestionario.Ativo = request.Ativo;
//            secaoQuestionario.DataAtualizacao = DateTime.UtcNow;

//            await _secaoQuestionarioRepository.UpdateAsync(secaoQuestionario);
//        }

//        public async Task<QuestaoDto> PostQuestao(QuestionarioPostQuestaoRequest request)
//        {
//            var secao = await _secaoQuestionarioRepository.GetSecaoQuestionarioById(request.IdSecao);

//            var createQuestao = await CreateQuestaoModel(request, secao);

//            await _questaoRepository.CreateAsync(createQuestao);

//            if (VerificaQuestaoDiferenteSimplesMultiplaCombobox(request.IdTipoCampo))
//                await CriaItemQuestaoPadrao(request, createQuestao);

//            var questaoDto = _mapper.Map<QuestaoDto>(createQuestao);

//            questaoDto.IdQuestao = createQuestao.Id;

//            return questaoDto;
//        }

//        public async Task PutQuestao(QuestaoQuestionarioPutRequest request)
//        {
//            var questao = await _questaoRepository.GetQuestaoById(request.IdQuestao);
//            var itemQuestao = await _itemQuestaoRepository.GetItemQuestaoByIdQuestao(request.IdQuestao);

//            if (questao == null)
//            {
//                _notificador.Handle(new Notificacao($"Questão com o id: {request.IdQuestao} não foi encontrada."));
//                return;
//            }

//            if (VerificaQuestaoDiferenteSimplesMultiplaCombobox(request.IdTipoQuestao))
//                await PutQuestaoDiferenteSimplesMultiplaCombobox(request, questao, itemQuestao);
//            else
//                PutQuestaoPadrao(request, questao);

//            await _questaoRepository.UpdateAsync(questao);
//        }

//        public async Task<GetQuestaoDto> GetQuestao(QuestaoGetFilter filter)
//        {
//            var questao = await _questaoRepository.GetQuestaoByIdQuestao(filter.IdQuestao);

//            if (questao == null)
//            {
//                _notificador.Handle(new Notificacao("O idQuestao passado não existe."));
//                return null;
//            }

//            var questionarioQuestao = _mapper.Map<GetQuestaoDto>(questao);
//            questionarioQuestao.IdQuestao = questao.Id;
//            questionarioQuestao.IdSecao = questao.IdSecao;
//            questionarioQuestao.Tipo = questao.IdTipoCampo;
//            questionarioQuestao.NomeQuestao = questao.Nome;
//            questionarioQuestao.Obrigatoria = questao.Obrigatoria;
//            if (VerificaQuestaoDiferenteSimplesMultiplaCombobox(questao.IdTipoCampo))
//            {
//                questionarioQuestao.PermiteUpload = questao.ItemQuestao.FirstOrDefault().PermiteUpload;
//                questionarioQuestao.UploadObrigatorio = questao.ItemQuestao.FirstOrDefault().UploadObrigatorio;
//                questionarioQuestao.PermiteObservacao = questao.ItemQuestao.FirstOrDefault().PermiteObservacao;
//                questionarioQuestao.ObservacaoObrigatoria = questao.ItemQuestao.FirstOrDefault().ObservacaoObrigatoria;
//            }

//            return questionarioQuestao;
//        }

//        public async Task<GetItemQuestaoDto> GetItemQuestao(GetItemQuestaoFilter filter)
//        {
//            var itemQuestao = await _itemQuestaoRepository.GetQuestaoByIdItemQuestao(filter.IdItemQuestao);

//            if (itemQuestao == null)
//            {
//                _notificador.Handle(new Notificacao("O idItemQuestao passado não existe."));
//                return null;
//            }

//            var questionarioitemQuestao = _mapper.Map<GetItemQuestaoDto>(itemQuestao);
//            questionarioitemQuestao.IdDesvio = itemQuestao.Desvio;
//            questionarioitemQuestao.NomeDesvio = itemQuestao.Questao.Nome;
//            questionarioitemQuestao.CodigoDesvio = itemQuestao.Questao.Numero;

//            return questionarioitemQuestao;
//        }

//        public async Task VincularQuestionarioSegmento(VincularQuestionarioSegmentoRequest request)
//        {
//            if (DesvincularTodosQuestionarioSegmento(request.IdsSegmentos))
//                await DesvincularQuestionarioSegmento(request.IdQuestionario);
//            else
//                await ValidarQuestionarioSegmento(request.IdsSegmentos, request.IdQuestionario);
//        }

//        public async Task VincularQuestionarioGrupo(VincularQuestionarioGrupoRequest request)
//        {
//            if (DesvincularTodosQuestionarioGrupo(request.IdsGrupo))
//                await DesvincularQuestionarioGrupo(request.IdQuestionario);
//            else
//                await ValidarQuestionarioGrupo(request.IdsGrupo, request.IdQuestionario);
//        }

//        public async Task ExcluirSecao(ExcluirSecaoQuestionarioRequest request)
//        {
//            foreach (var idSecao in request.IdsSecao)
//            {
//                var secao = await _secaoQuestionarioRepository.GetSecaoQuestionarioById(idSecao);

//                if (secao == null)
//                {
//                    _notificador.Handle(new Notificacao($"Nenhuma seção encontrada com este id: {idSecao}"));
//                    continue;
//                }

//                secao.Ativo = false;
//                secao.DataExclusao = DateTime.UtcNow;

//                await _secaoQuestionarioRepository.UpdateAsync(secao);
//            }
//        }

//        public async Task ExcluirQuestao(ExcluirQuestaoQuestionarioRequest request)
//        {
//            foreach (var idGrupo in request.IdsQuestao)
//            {
//                var questao = await _questaoRepository.GetQuestaoById(idGrupo);

//                if (questao == null)
//                {
//                    _notificador.Handle(new Notificacao($"Nenhuma questão encontrada com este id: {idGrupo}"));
//                    continue;
//                }

//                questao.Ativo = false;
//                questao.DataExclusao = DateTime.UtcNow;

//                await _questaoRepository.UpdateAsync(questao);
//            }
//        }

//        public async Task<IList<ListQuestaoQuestionarioDto>> ListQuestao(ListQuestaoQuestionarioRequest request)
//        {
//            return await _secaoQuestionarioRepository.GetListQuestaoByIdQuestionario(request.IdQuestionario, request.Pesquisa);
//        }

//        public async Task<IList<long>> GetVinculosSegmento(GetVinculosSegmentoRequest request)
//        {
//            return await _questionarioSegmentoRepository.GetVinculosSegmento(request.IdQuestionario);
//        }

//        public async Task<IList<long>> GetVinculosEquipe(GetVinculosEquipeRequest request)
//        {
//            return await _questionarioGrupoRepository.GetVinculosEquipe(request.IdQuestionario);
//        }

//        public async Task<IEnumerable<ListItemQuestaoDto>> ListItemQuestao(ListItemQuestaoRequest request)
//        {
//            return await _itemQuestaoRepository.List(request);
//        }

//        public async Task<IList<ListSecaoDto>> ListSecao(ListSecaoRequest request)
//        {
//            return await _secaoQuestionarioRepository.ListSecaoByIdQuestionario(request.IdQuestionario);
//        }

//        public async Task PostItemQuestao(PostItemQuestaoRequest request)
//        {
//            var questao = await _questaoRepository.GetQuestaoById(request.IdQuestao);

//            var numeroItemQuestao = await _itemQuestaoRepository.NumeroItemQuestao(request.IdQuestao);

//            var itemQuestao = _mapper.Map<ItemQuestao>(request);

//            itemQuestao.Ativo = true;
//            itemQuestao.Encerramento = request.Encerramento;
//            itemQuestao.Desvio = request.IdDesvio;
//            itemQuestao.Ordem = numeroItemQuestao + 1;
//            itemQuestao.Numero = $"{questao.Numero}.{numeroItemQuestao + 1}";
//            itemQuestao.DataCadastro = DateTime.UtcNow;
//            itemQuestao.DataAtualizacao = DateTime.UtcNow;

//            await _itemQuestaoRepository.CreateAsync(itemQuestao);
//        }

//        public async Task PutItemQuestao(PutItemQuestaoRequest request)
//        {
//            var itemQuestao = await _itemQuestaoRepository.GetItemQuestaoById(request.IdItemQuestao);

//            itemQuestao.Texto = request.Texto; 
//            itemQuestao.Encerramento = request.Encerramento;
//            itemQuestao.PermiteUpload = request.PermiteUpload;
//            itemQuestao.Desvio = request.IdDesvio;
//            itemQuestao.UploadObrigatorio = request.UploadObrigatorio;
//            itemQuestao.PermiteObservacao = request.PermiteObservacao;
//            itemQuestao.ObservacaoObrigatoria = request.ObservacaoObrigatoria;

//            await _itemQuestaoRepository.UpdateAsync(itemQuestao);
//        }

//        public async Task ExcluirItemQuestao(ExcluirItemQuestaoRequest request)
//        {
//            var itemQuestao = await _itemQuestaoRepository.GetItemQuestaoById(request.IdItemQuestao);

//            itemQuestao.Ativo = false;
//            itemQuestao.DataDesativacao = DateTime.UtcNow;

//            await _itemQuestaoRepository.UpdateAsync(itemQuestao);
//        }

//        public async Task<GetSecaoQuestionarioDto> GetSecao(QuestionarioGetSecaoFilter filter)
//        {
//            var secao = await _secaoQuestionarioRepository.GetSecaoByIdSecao(filter.IdSecao);

//            if (secao == null)
//            {
//                _notificador.Handle(new Notificacao("O idSecao passado não existe."));
//                return null;
//            }

//            var questionarioSecao = _mapper.Map<GetSecaoQuestionarioDto>(secao);

//            return questionarioSecao;
//        }

//        public async Task<PostCabecalhoRodapeResult> PostCabecalho(long idQuestionario, IFormFileCollection formFiles)
//        {
//            var fileName = $"writable/Checklist/Images/{_user.GetIdClienteCinq()}/Cabecalho_1.jpg";

//            var questionario = await _questionarioRepository.GetQuestionarioByIdAsync(idQuestionario);

//            if (ValidacaoQuestionarioEncontrado(idQuestionario, questionario))
//                return new PostCabecalhoRodapeResult(_notificador.ObterNotificacoes());

//            await UploadCabecalhoRodape(formFiles, fileName);

//            if (_notificador.TemNotificacao())
//                return new PostCabecalhoRodapeResult(_notificador.ObterNotificacoes());

//            questionario.Cabecalho = fileName;

//            await _questionarioRepository.UpdateAsync(questionario);

//            return new PostCabecalhoRodapeResult($"{_s3Extension.UrlS3}{fileName}");
//        }

//        public async Task<PostCabecalhoRodapeResult> PostRodape(long idQuestionario, IFormFileCollection formFiles)
//        {
//            var fileName = $"writable/Checklist/Images/{_user.GetIdClienteCinq()}/Rodape_1.jpg";

//            var questionario = await _questionarioRepository.GetQuestionarioByIdAsync(idQuestionario);

//            if (ValidacaoQuestionarioEncontrado(idQuestionario, questionario))
//                return new PostCabecalhoRodapeResult(_notificador.ObterNotificacoes());

//            await UploadCabecalhoRodape(formFiles, fileName);

//            if (_notificador.TemNotificacao())
//                return new PostCabecalhoRodapeResult(_notificador.ObterNotificacoes());

//            questionario.Rodape = fileName;

//            await _questionarioRepository.UpdateAsync(questionario);

//            return new PostCabecalhoRodapeResult($"{_s3Extension.UrlS3}{fileName}");
//        }

//        public async Task DeleteCabecalho(long idQuestionario)
//        {
//            var questionario = await _questionarioRepository.GetQuestionarioByIdAsync(idQuestionario);

//            questionario.Cabecalho = null;

//            await _questionarioRepository.UpdateAsync(questionario);
//        }

//        public async Task DeleteRodape(long idQuestionario)
//        {
//            var questionario = await _questionarioRepository.GetQuestionarioByIdAsync(idQuestionario);

//            questionario.Rodape = null;

//            await _questionarioRepository.UpdateAsync(questionario);
//        }

//        private static bool VerificaQuestaoDiferenteSimplesMultiplaCombobox(long idTipoCampo)
//        {
//            return idTipoCampo != (int)EQuestao.Simples && idTipoCampo != (int)EQuestao.Multipla && idTipoCampo != (int)EQuestao.Combobox;
//        }

//        private async Task CriaItemQuestaoPadrao(QuestionarioPostQuestaoRequest request, Questao createQuestao)
//        {
//            var questao = await _questaoRepository.GetQuestaoById(createQuestao.Id);

//            var numeroItemQuestao = await _itemQuestaoRepository.NumeroItemQuestao(createQuestao.Id);

//            var itemQuestaoPadrao = new ItemQuestao
//            {
//                IdQuestao = createQuestao.Id,
//                UploadObrigatorio = request.UploadObrigatorio,
//                PermiteUpload = request.PermiteUpload,
//                PermiteObservacao = request.PermiteObservacao,
//                ObservacaoObrigatoria = request.ObservacaoObrigatoria,
//                Texto = null,
//                Ordem = numeroItemQuestao + 1,
//                Numero = $"{questao.Numero}.{numeroItemQuestao + 1}",
//                Encerramento = request.Encerramento,
//                DataCadastro = DateTime.UtcNow,
//                DataAtualizacao = DateTime.UtcNow
//            };

//            await _itemQuestaoRepository.CreateAsync(itemQuestaoPadrao);
//        }

//        private async Task UploadCabecalhoRodape(IFormFileCollection formFiles, string fileName)
//        {
//            var filePath = Path.GetTempFileName();
//            await using var stream = new FileStream(filePath, FileMode.Create);

//            foreach (var formFile in formFiles)
//            {
//                if (formFile.Length <= 0)
//                    continue;

//                await formFile.CopyToAsync(stream);
//            }

//            if (ValidacaoCabecalhoRodape(stream))
//                return;

//            await _awsService.UploadFileAsync(stream, fileName, _s3Extension.Bucket);
//        }

//        private bool ValidacaoCabecalhoRodape(FileStream stream)
//        {
//            var image = Image.FromStream(stream);

//            if (image.Width > 2480 || image.Height > 300)
//                _notificador.Handle(new Notificacao("A imagem deve ter a dimensão máxima de 2480x300."));

//            return _notificador.TemNotificacao();
//        }

//        private async Task<Questao> CreateQuestaoModel(QuestionarioPostQuestaoRequest request, SecaoQuestionario secao)
//        {
//            return new Questao
//            {
//                Nome = request.Nome,
//                Numero = await NumeroQuestao(request, secao),
//                IdSecao = request.IdSecao,
//                Ativo = true,
//                Obrigatoria = request.Obrigatoria,
//                Ordem = await _questaoRepository.NumeroQuestao(request.IdSecao) + 1,
//                IdTipoCampo = request.IdTipoCampo,
//                Encerramento = request.Encerramento,
//                DataCadastro = DateTime.UtcNow,
//                DataAtualizacao = DateTime.UtcNow
//            };
//        }

//        private async Task PutQuestaoDiferenteSimplesMultiplaCombobox(QuestaoQuestionarioPutRequest request, Questao questao, ItemQuestao itemQuestao)
//        {
//            questao.IdSecao = request.IdSecao;
//            questao.Nome = request.Nome;
//            questao.IdTipoCampo = request.IdTipoQuestao;
//            questao.Obrigatoria = request.Obrigatoria;
//            questao.Encerramento = request.Encerramento;
//            questao.DataAtualizacao = DateTime.UtcNow;

//            itemQuestao.Texto = request.Texto;
//            itemQuestao.Encerramento = request.Encerramento;
//            itemQuestao.PermiteUpload = request.PermiteUpload;
//            itemQuestao.UploadObrigatorio = request.UploadObrigatorio;
//            itemQuestao.PermiteObservacao = request.PermiteObservacao;
//            itemQuestao.ObservacaoObrigatoria = request.ObservacaoObrigatoria;

//            await _itemQuestaoRepository.UpdateAsync(itemQuestao);
//        }

//        private static void PutQuestaoPadrao(QuestaoQuestionarioPutRequest request, Questao questao)
//        {
//            questao.IdSecao = request.IdSecao;
//            questao.Nome = request.Nome;
//            questao.Obrigatoria = request.Obrigatoria;
//            questao.Encerramento = request.Encerramento;
//            questao.DataAtualizacao = DateTime.UtcNow;
//        }

//        private async Task<string> NumeroQuestao(QuestionarioPostQuestaoRequest request, SecaoQuestionario secao)
//        {
//            var numeroQuestao = await _questaoRepository.NumeroQuestao(request.IdSecao) + 1;

//            if (secao.Padrao)
//                return $"{numeroQuestao}";

//            return $"{secao.Numero}.{numeroQuestao}";
//        }

//        private async Task<SecaoQuestionario> CreateSecaoModel(QuestionarioPostSecaoRequest request)
//        {
//            return new SecaoQuestionario
//            {
//                Nome = request.Nome,
//                IdQuestionario = request.IdQuestionario,
//                Ativo = true,
//                Padrao = request.Padrao,
//                Numero = await NumeroSecao(request),
//                DataCadastro = DateTime.UtcNow,
//                DataAtualizacao = DateTime.UtcNow
//            };
//        }

//        private async Task<long> NumeroSecao(QuestionarioPostSecaoRequest request)
//        {
//            return await _secaoQuestionarioRepository.NumeroSecao(request.IdQuestionario) + 1;
//        }

//        private static bool VerificaFimVigenciaNull(DateTime? fimVigencia)
//        {
//            return fimVigencia == new DateTime() || fimVigencia == null;
//        }

//        private Questionario CreateQuestionarioModel(QuestionarioPostRequest request)
//        {
//            if (request.InicioVigencia == new DateTime())
//                request.InicioVigencia = DateTime.UtcNow;

//            if (VerificaFimVigenciaNull(request.FimVigencia))
//                request.FimVigencia = DateTime.UtcNow.AddYears(100);

//            var modelQuestionario = new Questionario
//            {
//                IdUsuarioCriador = _user.GetUserId(),
//                IdStatus = (long)EStatusQuestionario.Incompleto,
//                Nome = request.Nome,
//                NomeMobile = request.Nome,
//                NumeroInicial = request.NumeroInicial,
//                InicioVigencia = request.InicioVigencia,
//                FimVigencia = request.FimVigencia,
//                QualidadeImagem = 3,
//                Obrigatorio = request.Obrigatorio,
//                EnviarQuestionarioEmail = request.EnviarQuestionarioEmail,
//                ConexaoInternetObrigatoria = request.ConexaoInternetObrigatoria,
//                ExibirNovoRegistro = request.ExibirNovoRegistro,
//                DataCadastro = DateTime.UtcNow,
//                DataAtualizacao = DateTime.UtcNow,
//                Descricao = request.Descricao
//            };

//            return modelQuestionario;
//        }

//        private static bool QuestionarioCompletoNoPrazo(Questionario questionario)
//        {
//            return questionario.IdStatus == (int)EStatusQuestionario.NaoPublicado && DateTime.UtcNow > questionario.InicioVigencia && questionario.FimVigencia < DateTime.UtcNow;
//        }

//        private static bool QuestionarioIncompleto(Questionario questionario)
//        {
//            return questionario.IdStatus == (int)EStatusQuestionario.Incompleto;
//        }

//        private bool NenhumQuestionarioEncontrado(IEnumerable<Questionario> questionarioList, IEnumerable<long> idsQuestionarios)
//        {
//            if (questionarioList.Any())
//                return false;

//            _notificador.Handle(new Notificacao($"Nenhum questionário encontrado com este(s) id(s) {string.Join(',', idsQuestionarios)}"));
//            return true;

//        }

//        private bool ValidacaoQuestionarioEncontrado(long idQuestionario, Questionario questionario)
//        {
//            if (questionario != null)
//                return false;

//            _notificador.Handle(new Notificacao($"Questionario com o Id: {idQuestionario} não encontrado"));
//            return true;
//        }

//        private async Task DesvincularQuestionarioSegmento(long idQuestionario)
//        {
//            var listQuestionarioSegmento = await _questionarioSegmentoRepository.GetQuestionarioSegmentoAtivoByIdQuestionario(idQuestionario);

//            foreach (var questionarioSegmento in listQuestionarioSegmento)
//            {
//                questionarioSegmento.Excluido = true;
//                questionarioSegmento.DataAtualizacao = DateTime.UtcNow;
//            }

//            await _questionarioSegmentoRepository.UpdateArrayAsync(listQuestionarioSegmento);
//        }

//        private static bool DesvincularTodosQuestionarioSegmento(List<long?> idsSegmentos)
//        {
//            return idsSegmentos == null;
//        }

//        private async Task ValidarQuestionarioSegmento(List<long?> idsSegmentos, long idQuestionario)
//        {
//            var listQuestionarioSegmentoAtivo = await _questionarioSegmentoRepository.GetQuestionarioSegmentoAtivoByIdQuestionario(idQuestionario);

//            var listIdsSegmentos = new List<long?>();

//            foreach (var idSegmento in idsSegmentos)
//            {
//                var vinculoEncontrado = listQuestionarioSegmentoAtivo.FirstOrDefault(x => x.IdSegmento == idSegmento);

//                if (vinculoEncontrado == null)
//                    await VerificarVinculoQuestionarioSegmento(idSegmento, idQuestionario, listIdsSegmentos);

//                listQuestionarioSegmentoAtivo.Remove(vinculoEncontrado);
//            }

//            if (listQuestionarioSegmentoAtivo.Any())
//                await DesvincularQuestionarioSegmentoAsync(listQuestionarioSegmentoAtivo);

//            if (listIdsSegmentos.Any())
//                await CreateVinculoQuestionarioSegmentoAsync(listIdsSegmentos, idQuestionario);
//        }

//        private async Task CreateVinculoQuestionarioSegmentoAsync(List<long?> listIdsSegmentos, long idQuestionario)
//        {
//            var listQuestionarioSegmento = new List<QuestionarioSegmento>();

//            foreach (var idSegmento in listIdsSegmentos)
//            {
//                var questionarioSegmento = new QuestionarioSegmento
//                {
//                    IdQuestionario = idQuestionario,
//                    IdSegmento = (long)idSegmento,
//                    DataCadastro = DateTime.UtcNow,
//                    DataAtualizacao = DateTime.UtcNow
//                };

//                listQuestionarioSegmento.Add(questionarioSegmento);
//            }

//            await _questionarioSegmentoRepository.CreateArrayAsync(listQuestionarioSegmento);
//        }

//        private async Task DesvincularQuestionarioSegmentoAsync(IList<QuestionarioSegmento> listQuestionarioSegmentoAtivo)
//        {
//            foreach (var questionarioSegmento in listQuestionarioSegmentoAtivo)
//            {
//                questionarioSegmento.Excluido = true;
//                questionarioSegmento.DataAtualizacao = DateTime.UtcNow;
//            }

//            await _questionarioSegmentoRepository.UpdateArrayAsync(listQuestionarioSegmentoAtivo);
//        }

//        private async Task VerificarVinculoQuestionarioSegmento(long? idSegmento, long idQuestionario, List<long?> listIdsSegmentos)
//        {
//            var vinculoQuestionarioSegmento = await _questionarioSegmentoRepository.GetQuestionarioSegmentoByIdQuestionarioAndIdSegmento((long)idSegmento, idQuestionario);

//            if (vinculoQuestionarioSegmento == null)
//                listIdsSegmentos.Add(idSegmento);
//            else
//            {
//                vinculoQuestionarioSegmento.Excluido = false;
//                vinculoQuestionarioSegmento.DataAtualizacao = DateTime.UtcNow;

//                await _questionarioSegmentoRepository.UpdateAsync(vinculoQuestionarioSegmento);
//            }
//        }

//        private async Task DesvincularQuestionarioGrupo(long idQuestionario)
//        {
//            var listQuestionarioGrupo = await _questionarioGrupoRepository.GetQuestionarioGrupoByIdQuestionario(idQuestionario);

//            foreach (var questionarioGrupo in listQuestionarioGrupo)
//            {
//                questionarioGrupo.Excluido = true;
//                questionarioGrupo.DataAtualizacao = DateTime.UtcNow;
//            }

//            await _questionarioGrupoRepository.UpdateArrayAsync(listQuestionarioGrupo);
//        }

//        private bool DesvincularTodosQuestionarioGrupo(List<long?> idsGrupo)
//        {
//            return idsGrupo == null;
//        }

//        private async Task ValidarQuestionarioGrupo(List<long?> idsGrupo, long idQuestionario)
//        {
//            var listQuestionarioGrupo = await _questionarioGrupoRepository.GetQuestionarioGrupoByIdQuestionario(idQuestionario);

//            var listIdsGrupo = new List<long?>();

//            foreach (var idGrupo in idsGrupo)
//            {
//                var vinculoEncontrado = listQuestionarioGrupo.FirstOrDefault(x => x.IdGrupo == idGrupo);

//                if (vinculoEncontrado == null)
//                    await VerificarVinculoQuestionarioGrupo(idGrupo, idQuestionario, listIdsGrupo);

//                listQuestionarioGrupo.Remove(vinculoEncontrado);
//            }

//            if (listQuestionarioGrupo.Any())
//                await DesvincularQuestionarioGrupoAsync(listQuestionarioGrupo);

//            if (listIdsGrupo.Any())
//                await CreateVinculoQuestionarioGrupoAsync(listIdsGrupo, idQuestionario);
//        }

//        private async Task CreateVinculoQuestionarioGrupoAsync(List<long?> listIdsGrupo, long idQuestionario)
//        {
//            var listQuestionarioSegmento = new List<QuestionarioGrupo>();

//            foreach (var idGrupo in listIdsGrupo)
//            {
//                var questionarioGrupo = new QuestionarioGrupo
//                {
//                    IdQuestionario = idQuestionario,
//                    IdGrupo = (long)idGrupo,
//                    DataCadastro = DateTime.UtcNow,
//                    DataAtualizacao = DateTime.UtcNow
//                };

//                listQuestionarioSegmento.Add(questionarioGrupo);
//            }

//            await _questionarioGrupoRepository.CreateArrayAsync(listQuestionarioSegmento);
//        }

//        private async Task DesvincularQuestionarioGrupoAsync(IList<QuestionarioGrupo> listQuestionarioGrupo)
//        {
//            foreach (var questionarioGrupo in listQuestionarioGrupo)
//            {
//                questionarioGrupo.Excluido = true;
//                questionarioGrupo.DataAtualizacao = DateTime.UtcNow;
//            }

//            await _questionarioGrupoRepository.UpdateArrayAsync(listQuestionarioGrupo);
//        }

//        private async Task VerificarVinculoQuestionarioGrupo(long? idGrupo, long idQuestionario, List<long?> listIdsGrupo)
//        {
//            var vinculoQuestionarioGrupo = await _questionarioGrupoRepository.GetQuestionarioGrupoByIdQuestionarioAndIdGrupo((long)idGrupo, idQuestionario);

//            if (vinculoQuestionarioGrupo == null)
//                listIdsGrupo.Add(idGrupo);
//            else
//            {
//                vinculoQuestionarioGrupo.Excluido = false;
//                vinculoQuestionarioGrupo.DataAtualizacao = DateTime.UtcNow;

//                await _questionarioGrupoRepository.UpdateAsync(vinculoQuestionarioGrupo);
//            }
//        }
//    }
//}