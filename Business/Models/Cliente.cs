using System;

namespace Business.Models
{
    public class Cliente : Entity
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} - Nome: {Nome}";
        }
    }
}