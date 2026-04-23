using LojaProdutosCurso.Data;
using LojaProdutosCurso.Dto.Produto;
using LojaProdutosCurso.Models;
using Microsoft.EntityFrameworkCore;

namespace LojaProdutosCurso.Services.Produto
{
    public class ProdutoService : IProdutoInterface
    {

        private readonly DataContext _context;

        private readonly string _sistema;
        public ProdutoService(DataContext context, IWebHostEnvironment sistema)
        {
            _context = context;
            _sistema = sistema.WebRootPath;
        }

        public async Task<List<ProdutoModel>> BuscarProdutoFiltro(string? pesquisar)
        {
            try
            {

                // Utiliza o método Include para incluir a categoria relacionada e o método Where para filtrar os produtos
                // com base no nome ou marca que contenham a string de pesquisa, e retorna a lista de produtos encontrados
                var produtos = await _context.Produtos
                                    .Include(x => x.Categoria)
                                    .Where(p => p.Nome.Contains(pesquisar) || p.Marca.Contains(pesquisar))
                                    .ToListAsync();

                return produtos;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoModel> BuscarProdutoPorId(int id)
        {
            // Busca o produto pelo ID, incluindo a categoria relacionada, e retorna o produto encontrado
            try
            {
                // Utiliza o método FirstOrDefaultAsync para buscar o produto com o ID especificado, e inclui a categoria relacionada usando o método Include
                var produto = await _context.Produtos
                                        .Include(x => x.Categoria)
                                        .FirstOrDefaultAsync(p => p.Id == id); // Busca o produto pelo ID, incluindo a categoria relacionada

                return produto;
            }
            // Captura qualquer exceção que ocorra durante o processo e lança uma nova exceção com a mensagem original
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProdutoModel>> BuscarProdutos()
        {
            try
            {


                return await _context.Produtos.Include(c => c.Categoria).ToListAsync();


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }
        }

        public async Task<ProdutoModel> Cadastrar(CriarProdutoDto criarProdutoDto, IFormFile foto)
        {
            try
            {
                // Gera o nome do caminho da imagem e salva a imagem no servidor
                var nomeCaminhoImagem = GeraCaminhoArquivo(foto);

                // Cria um novo objeto ProdutoModel com os dados do DTO e o nome do caminho da imagem
                var produto = new ProdutoModel
                {
                    Nome = criarProdutoDto.Nome,
                    Marca = criarProdutoDto.Marca,
                    Valor = criarProdutoDto.Valor,
                    CategoriaModelId = criarProdutoDto.CategoriaModelId,
                    Foto = nomeCaminhoImagem,
                    QuantidadeEstoque = criarProdutoDto.QuantidadeEstoque
                };

                // Adiciona o produto ao contexto e salva as alterações no banco de dados
                _context.Produtos.Add(produto);
                await _context.SaveChangesAsync();

                return produto;


            }
            // Captura qualquer exceção que ocorra durante o processo e lança uma nova exceção com a mensagem original
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoModel> Editar(EditarProdutoDto editarProdutoDto, IFormFile? foto)
        {
            try
            {

                var produto = await BuscarProdutoPorId(editarProdutoDto.Id);

                // Inicializa a variável nomeCaminhoImagem como uma string vazia, que será usada para armazenar o nome do caminho da nova imagem, caso uma nova imagem seja fornecida
                var nomeCaminhoImagem = "";
                if (foto != null)
                {

                    string caminhoCapaExistente = _sistema + "\\imagem\\" + produto.Foto; // Gera o caminho completo do arquivo da imagem existente, utilizando o caminho do sistema, a pasta "imagem" e o nome do arquivo da imagem existente

                    // Verifica se o arquivo da imagem existente existe, e se existir, exclui o arquivo para evitar acúmulo de arquivos antigos no servidor
                    if (File.Exists(caminhoCapaExistente))
                    {
                        File.Delete(caminhoCapaExistente);
                    }

                    // Gera o nome do caminho da nova imagem e salva a nova imagem no servidor
                    nomeCaminhoImagem = GeraCaminhoArquivo(foto);

                }

                // Atualiza os campos do produto com os dados do DTO e o nome do caminho da imagem, se uma nova imagem foi fornecida
                produto.Nome = editarProdutoDto.Nome;
                produto.Marca = editarProdutoDto.Marca;
                produto.Valor = editarProdutoDto.Valor;
                produto.QuantidadeEstoque = editarProdutoDto.QuantidadeEstoque;
                produto.CategoriaModelId = editarProdutoDto.CategoriaModelId;

                // Se uma nova imagem foi fornecida, atualiza o campo Foto do produto com o nome do caminho da nova imagem
                if (nomeCaminhoImagem != "")
                {
                    produto.Foto = nomeCaminhoImagem;
                }

                // Atualiza o produto no contexto e salva as alterações no banco de dados
                _context.Update(produto);
                await _context.SaveChangesAsync();

                return produto;
            }
            catch (Exception ex) // Captura qualquer exceção que ocorra durante o processo e lança uma nova exceção com a mensagem original
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoModel> Remover(int id)
        {
            try
            {

                var produto = await BuscarProdutoPorId(id); // Busca o produto pelo ID para obter as informações necessárias para a remoção, como o nome do caminho da imagem

                _context.Remove(produto); // Remove o produto do contexto, marcando-o para exclusão no banco de dados
                await _context.SaveChangesAsync(); // Salva as alterações no banco de dados, efetivando a exclusão do produto

                return produto;

            }
            catch (Exception ex) // Captura qualquer exceção que ocorra durante o processo e lança uma nova exceção com a mensagem original
            {
                throw new Exception(ex.Message);
            }
        }

        // Método privado para gerar o caminho do arquivo da imagem e salvar a imagem no servidor
        private string GeraCaminhoArquivo(IFormFile foto)
        {

            // Gera um código único para evitar conflitos de nomes de arquivos
            var codigoUnico = Guid.NewGuid().ToString();

            // Gera o nome do caminho da imagem, removendo espaços e convertendo para minúsculas, e adicionando o código único para garantir que o nome seja único
            var nomeCaminhoImagem = foto.FileName.Replace(" ", "").ToLower() + codigoUnico + ".png";

            // Define o caminho para salvar as imagens, utilizando o caminho do sistema e uma pasta "imagem"
            var caminhoParaSalvarImagens = _sistema + "\\imagem\\";

            // Verifica se a pasta para salvar as imagens existe, e se não existir, cria a pasta
            if (!Directory.Exists(caminhoParaSalvarImagens))
            {
                Directory.CreateDirectory(caminhoParaSalvarImagens);
            }

            using (var stream = File.Create(caminhoParaSalvarImagens + nomeCaminhoImagem))
            {
                foto.CopyToAsync(stream).Wait();
            }

            return nomeCaminhoImagem;

        }
    }
}