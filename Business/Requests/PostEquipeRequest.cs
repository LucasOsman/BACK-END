namespace Business.Requests
{
    public class PostEquipeRequest
    {
        public string Nome { get; set; }
        public string Observacao { get; set; }
        public long IdUsuario { get; set; }
    }
}