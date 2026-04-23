using LojaProdutosCurso.Data;
using LojaProdutosCurso.Models;
using Microsoft.EntityFrameworkCore;

namespace LojaProdutosCurso.Services.Categoria
{
    public class CategoriaService : ICategoriaInterface
    {
        // Injeção de dependência do DataContext para acessar o banco de dados
        private readonly DataContext _context;

        // Construtor para injetar o DataContext
        public CategoriaService(DataContext context)
        {
            // Atribui o DataContext injetado à variável local para uso nos métodos da classe
            _context = context;
        }

        // Implementação do método para buscar todas as categorias
        public async Task<List<CategoriaModel>> BuscarCategorias()
        {
            // Tenta buscar as categorias do banco de dados
            try
            {
                // Usa o Entity Framework para buscar todas as categorias e convertê-las em uma lista
                var categorias = await _context.Categorias.ToListAsync();
                return categorias;
            }
            // Captura qualquer exceção que possa ocorrer durante a operação e lança uma nova exceção com a mensagem original
            catch (Exception ex)
            {
                // Lança uma nova exceção com a mensagem original para que o chamador possa lidar com ela
                throw new Exception(ex.Message);
            }

        }
    }
}   
