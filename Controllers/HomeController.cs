using LojaProdutosCurso.Filtros;
using LojaProdutosCurso.Models;
using LojaProdutosCurso.Services.Produto;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LojaProdutosCurso.Controllers
{
    [UsuarioLogado]
    public class HomeController : Controller
    { 
        private readonly IProdutoInterface _produtoInterface; //variável para acessar os métodos da interface

        public HomeController(IProdutoInterface produtoInterface) //injeção de dependência
        {
            _produtoInterface = produtoInterface; //atribuição da variável com a instância da interface
        }


        public async Task<IActionResult> Index(string? pesquisar) //método para exibir a página inicial, com a opção de pesquisar produtos
        {
            List<ProdutoModel> produtos = new List<ProdutoModel>(); //lista para armazenar os produtos que serão exibidos na página

            if (pesquisar == null) //se não houver pesquisa, exibe todos os produtos
            {
                produtos = await _produtoInterface.BuscarProdutos(); //chama o método para obter todos os produtos
            }
            else //se houver pesquisa, exibe os produtos que correspondem à pesquisa
            {
                produtos = await _produtoInterface.BuscarProdutoFiltro(pesquisar); //chama o método para obter os produtos que correspondem à pesquisa
            }

            return View(produtos);
        }


    }
}
