using LojaProdutosCurso.Dto.Login;
using LojaProdutosCurso.Dto.Produto;
using LojaProdutosCurso.Dto.Usuario;
using LojaProdutosCurso.Models;

namespace LojaProdutosCurso.Services.Usuario
{
    public interface IUsuarioInterface
    {
        Task<List<UsuarioModel>> BuscarUsuario(); // Buscar todos os usuários
        Task<UsuarioModel> BuscarUsuarioPorId(int id); // Buscar um usuário por ID
        Task<bool> VerificaSeExisteEmail(CriarUsuarioDto criarUsuarioDto); // Verificar se o email já existe antes de cadastrar um novo usuário
        Task<CriarUsuarioDto> Cadastrar(CriarUsuarioDto criarUsuarioDto); // Cadastrar um novo usuário

        Task<UsuarioModel> Editar(EditarUsuarioDto editarUsuarioDto); // Editar um usuário existente
        Task<UsuarioModel> Login(LoginUsuarioDto loginUsuarioDto); // Login de um usuário existente

    }
}
