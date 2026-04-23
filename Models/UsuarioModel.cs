using LojaProdutosCurso.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LojaProdutosCurso.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public byte[] SenhaHash { get; set; } 
        public byte[] SenhaSalt { get; set; } // Diferenciar os hashes de senhas para usuários diferentes
        public CargoEnum Cargo { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime DataAlteracao { get; set; } = DateTime.Now;

        [ValidateNever]
        public EnderecoModel Endereco
        {
            get; set;


        }
    }
}
