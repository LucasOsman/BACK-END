namespace Business.Requests
{
    public class PostOrdemServicoRequest
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public long IdEquipe { get; set; }
        public int Status { get; set; }
    }
}