using System;

namespace Business.Models
{
    public class Equipe : Entity
    {
        public string Nome { get; set; }
        public string Observacao { get; set; }
        public long IdUsuario { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizao { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} - Nome: {Nome} - IdUsuario: {IdUsuario}";
        }
    }
}