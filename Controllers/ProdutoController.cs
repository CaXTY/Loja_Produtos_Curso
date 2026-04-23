using LojaProdutosCurso.Dto.Produto;
using LojaProdutosCurso.Filtros;
using LojaProdutosCurso.Services.Categoria;
using LojaProdutosCurso.Services.Produto;
using Microsoft.AspNetCore.Mvc;

namespace LojaProdutosCurso.Controllers
{
    [UsuarioLogado]
    public class ProdutoController : Controller
    {
        // Injeção de dependência dos serviços de produto e categoria para acessar os dados necessários
        private readonly IProdutoInterface _produtoInterface;

        // Injeção de dependência do serviço de categoria para acessar as categorias necessárias para o cadastro de produtos
        private readonly ICategoriaInterface _categoriaInterface;

        // Construtor para injetar os serviços de produto e categoria
        public ProdutoController(IProdutoInterface produtoInterface,
                                 ICategoriaInterface categoriaInterface)
        {
            // Atribui os serviços injetados às variáveis locais para uso nos métodos da classe
            _produtoInterface = produtoInterface;
            _categoriaInterface = categoriaInterface;
        }


        // Ação para exibir a lista de produtos
        [UsuarioLogadoAdm]
        public async Task<IActionResult> Index()
        {
            // Busca os produtos para exibir na view de listagem
            var produtos = await _produtoInterface.BuscarProdutos();

            // Retorna a view de listagem com os produtos
            return View(produtos);
        }


        // Ação para exibir a página de cadastro de produto
        [UsuarioLogadoAdm]
        public async Task<IActionResult> Cadastrar()
        {
            // Busca as categorias para exibir no dropdown da view de cadastro
            ViewBag.Categorias = await _categoriaInterface.BuscarCategorias();

            // Retorna a view de cadastro
            return View();
        }

        [UsuarioLogadoAdm]
        public  async Task<IActionResult> Remover(int id)
        {
            var produto = await _produtoInterface.Remover(id); // Chama o método de remoção do serviço de produto para remover o produto com o ID fornecido

            return RedirectToAction("Index", "Produto"); // Redireciona para a ação "Index" do controlador "Produto" para exibir a lista atualizada de produtos após a remoção

        }

        public async Task<IActionResult> Detalhes(int id)
        {
            // Busca o produto pelo ID para exibir na view de detalhes
            var produto = await _produtoInterface.BuscarProdutoPorId(id);

            // Retorna a view de detalhes com os dados do produto
            return View(produto);
        }

        [UsuarioLogadoAdm]
        public async Task<IActionResult> Editar(int id)
        {
            // Busca o produto pelo ID para exibir na view de edição
            var produto = await _produtoInterface.BuscarProdutoPorId(id);

            //if (produto == null)
            //{
            //    TempData["MensagemErro"] = "Produto não encontrado.";
            //    return RedirectToAction("Index");
            //}

            // Preenche o DTO de edição com os dados do produto para exibir na view de edição
            var editarProdutoDto = new EditarProdutoDto
            {
                Nome = produto.Nome,
                Marca = produto.Marca,
                Foto = produto.Foto,
                Valor = produto.Valor,
                QuantidadeEstoque = produto.QuantidadeEstoque,
                CategoriaModelId = produto.CategoriaModelId,

            };

            // Busca as categorias para exibir no dropdown da view de edição
            ViewBag.Categorias = await _categoriaInterface.BuscarCategorias();

            // Retorna a view de edição com os dados do produto preenchidos
            return View(editarProdutoDto);
        }

        // Ação para processar o formulário de cadastro de produto
        [HttpPost]
        [UsuarioLogadoAdm]
        public async Task<IActionResult> Cadastrar(CriarProdutoDto criarProdutoDto, IFormFile foto)
        {
            // Verifica se os dados do formulário são válidos
            if (ModelState.IsValid)
            {
                var produto = await _produtoInterface.Cadastrar(criarProdutoDto, foto);
                //Sucesso
                TempData["MensagemSucesso"] = "Produto cadastrado com sucesso!"; // Armazena uma mensagem de sucesso na TempData para exibir na próxima requisição
                return RedirectToAction("Index", "Produto");
            }
            else
            {
                //Erro 
                TempData["MensagemErro"] = "Erro ao cadastrar o produto. Verifique os dados e tente novamente."; // Armazena uma mensagem de erro na TempData para exibir na próxima requisição
                ViewBag.Categorias = await _categoriaInterface.BuscarCategorias();
                return View(criarProdutoDto);
            }
        }

        [HttpPost]
        [UsuarioLogadoAdm]
        public async Task<IActionResult> Editar(EditarProdutoDto editarProdutoDto, IFormFile? foto) // Permite que a foto seja opcional, caso o usuário não queira alterar a foto existente
        {
            if (ModelState.IsValid)
            {
                var produto = await _produtoInterface.Editar(editarProdutoDto, foto);
                //Sucesso
                TempData["MensagemSucesso"] = "Produto editado com sucesso!";
                return RedirectToAction("Index", "Produto");
            }
            else
            {
              
                ViewBag.Categorias = await _categoriaInterface.BuscarCategorias();
                //Erro 
                TempData["MensagemErro"] = "Erro ao editar o produto. Verifique os dados e tente novamente.";
                return View(editarProdutoDto);
            }
        }
    }

}
