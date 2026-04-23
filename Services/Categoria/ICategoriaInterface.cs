using LojaProdutosCurso.Models;

namespace LojaProdutosCurso.Services.Categoria
{
    public interface ICategoriaInterface
    {
        // Defina os métodos que a CategoriaService deve implementar
        Task<List<CategoriaModel>> BuscarCategorias();

    }
}
