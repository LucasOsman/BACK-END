namespace Business.Requests
{
    public class PutUsuarioRequest
    {
        public long Id { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
    }
}