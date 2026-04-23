namespace LojaProdutosCurso.Services.Autenticacao
{
    public interface IAutenticaoInterface
    {
        void CriarSenhaHash(string senha, out byte[] senhaHash, out byte[] senhaSalt);
        bool VerificaLogin(string senha, byte[] senhaHash, byte[] senhaSalt);
    }
}
