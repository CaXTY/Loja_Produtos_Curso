using LojaProdutosCurso.Models;

namespace LojaProdutosCurso.Services.Estoque
{
    public interface IEstoqueInterface
    {
        Task<ProdutosBaixadosModel> CriarRegistro(int idProduto);
        List<RegistroProdutosModel> ListagemRegistros();
    }
}