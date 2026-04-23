using LojaProdutosCurso.Data;
using LojaProdutosCurso.Models;
using LojaProdutosCurso.Services.Produto;
using Microsoft.EntityFrameworkCore;


namespace LojaProdutosCurso.Services.Estoque
{
    public class EstoqueService : IEstoqueInterface
    {
        private readonly DataContext _context;
        private readonly IProdutoInterface _produtoInterface;

        public EstoqueService(DataContext context, IProdutoInterface produtoInterface)
        {
            _context = context;
            _produtoInterface = produtoInterface;
        }


        public async Task<ProdutosBaixadosModel> CriarRegistro(int idProduto)
        {
            try
            {

                var produto = await _produtoInterface.BuscarProdutoPorId(idProduto);

                var produtoBaixado = new ProdutosBaixadosModel()
                {
                    Produto = produto,
                    ProdutoModelId = idProduto
                };

                _context.Add(produtoBaixado);
                await _context.SaveChangesAsync();

                //Baixar Estoque
                BaixarEstoque(produto);

                _context.Update(produto);
                await _context.SaveChangesAsync();

                return produtoBaixado;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void BaixarEstoque(ProdutoModel produto)
        {
            produto.QuantidadeEstoque--;
        }

        public List<RegistroProdutosModel> ListagemRegistros()
        {
            try
            {

                // Agrupar os registros por categoria e data da baixa, e calcular o total para cada grupo
                var resultado = from c in _context.ProdutosBaixados.Include(x => x.Produto)
                                                                   .Include(y => y.Produto.Categoria)
                                                                   .ToList()
                                group c by new { c.Produto.CategoriaModelId, c.DataDaBaixa } into total
                                select new
                                {
                                    ProdutoId = total.First().Produto.Categoria.Id,
                                    CategoriaNome = total.First().Produto.Categoria.Nome,
                                    DataCompra = total.First().DataDaBaixa,
                                    Total = total.Sum(c => c.Produto.Valor)
                                };

                // Calcular o total geral de todas as compras
                var totalGeral = _context.ProdutosBaixados.Include(x => x.Produto)
                                                           .Include(y => y.Produto.Categoria)
                                                           .Sum(c => c.Produto.Valor);


                // Criar a lista de registros
                List<RegistroProdutosModel> lista = new List<RegistroProdutosModel>();

                // Preencher a lista com os resultados do agrupamento
                foreach (var result in resultado)
                {
                    var registro = new RegistroProdutosModel()
                    {
                        ProdutoId = result.ProdutoId,
                        CategoriaNome = result.CategoriaNome,
                        DataCompra = result.DataCompra,
                        Total = result.Total,
                        TotalGeral = totalGeral

                    };

                    lista.Add(registro);
                }

                return lista;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}