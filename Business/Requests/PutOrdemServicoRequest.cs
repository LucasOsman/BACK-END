namespace Business.Requests
{
    public class PutOrdemServicoRequest
    {
        public long IdOrdemServico { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public long IdEquipe { get; set; }
        public int Status { get; set; }
    }
}