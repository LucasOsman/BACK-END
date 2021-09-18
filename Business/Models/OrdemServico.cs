using System;

namespace Business.Models
{
    public class OrdemServico : Entity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public long IdEquipe { get; set; }
        public int Status { get; set; }
        public DateTime DataCadastro { get; set; }

        public override string ToString()
        {
            return $"Nome: {Nome}";
        }
    }
}