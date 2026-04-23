using System.Security.Cryptography;

namespace LojaProdutosCurso.Services.Autenticacao
{
    public class AutenticacaoService : IAutenticaoInterface
    {
        public void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt)
        {
            // Gerar um hash da senha usando HMACSHA512
            using (var hmac = new HMACSHA512())
            {
                senhaSalt = hmac.Key; // O salt é a chave gerada pelo HMAC
                senhaHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha)); // O hash é o resultado da computação do hash da senha
            }
        }

        public bool VerificaLogin(string senha, byte[] senhaHash, byte[] senhaSalt)
        {
            // Verificar se a senha fornecida corresponde ao hash armazenado usando o salt armazenado
            using (var hmac = new HMACSHA512(senhaSalt))
            {
                var computedHas = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha)); // Computa o hash da senha fornecida usando o salt armazenado
                return computedHas.SequenceEqual(senhaHash); // Compara o hash computado com o hash armazenado
            }
        }
    }
}
