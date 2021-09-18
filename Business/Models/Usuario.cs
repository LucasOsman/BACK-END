using System;

namespace Business.Models
{
    public class Usuario : Entity
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} - Nome: {Nome} - Login: {Login}";
        }
    }
}