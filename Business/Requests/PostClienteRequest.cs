using System;

namespace Business.Requests
{
    public class PostClienteRequest
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
    }
}