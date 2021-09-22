using System;

namespace Business.DTOs
{
    public class UsuarioDto
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}