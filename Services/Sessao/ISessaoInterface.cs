using LojaProdutosCurso.Models;

namespace LojaProdutosCurso.Services.Sessao
{
    public interface ISessaoInterface
    {

        void CriarSessao(UsuarioModel usuario);
        void RemoverSessao();
        UsuarioModel BuscarSessao();

    }
}
