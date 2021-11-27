namespace Business.Requests
{
    public class PutEquipeRequest
    {
        public long IdEquipe { get; set; }
        public string Nome { get; set; }
        public string Observacao { get; set; }
        public long IdUsuario { get; set; }
    }
}