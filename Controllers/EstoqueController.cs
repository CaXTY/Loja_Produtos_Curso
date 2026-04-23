using ClosedXML.Excel;
using LojaProdutosCurso.Filtros;
using LojaProdutosCurso.Services.Estoque;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LojaProdutosCurso.Controllers
{
    [UsuarioLogado]
    public class EstoqueController : Controller
    {
        private readonly IEstoqueInterface _estoqueInterface;

        public EstoqueController(IEstoqueInterface estoqueInterface) 
        {
            _estoqueInterface = estoqueInterface;
        }


        [UsuarioLogadoAdm]
        public IActionResult Index()
        {
            var registros = _estoqueInterface.ListagemRegistros(); // Buscar os registros de estoque usando o serviço de estoque e armazená-los em uma variável chamada "registros"
            return View(registros);
        }

        public IActionResult GerarRelatorio()
        {
            // Buscar os dados necessários para o relatório
            var dados = BuscarDados();

            //Retornar a view do relatório, passando os dados
            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.AddWorksheet(dados, "Dados Vendas"); // Adicionar os dados ao workbook, nomeando a planilha como "Dados Vendas"

                using (MemoryStream ms = new MemoryStream()) // Criar um MemoryStream para armazenar o arquivo Excel em memória
                {
                    workbook.SaveAs(ms);
                    ms.Seek(0, SeekOrigin.Begin); // Voltar para o início do stream
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Relatorio_Vendas.xlsx"); // Retornar o arquivo Excel como um download, com o nome "Relatorio_Vendas.xlsx"
                }
            }
        }


        private DataTable BuscarDados()
        {
            // Lógica para buscar os dados do relatório
            // Pode ser uma consulta ao banco de dados ou outra fonte de dados
            // Retornar os dados em um formato adequado, como um DataTable ou uma lista de objetos

            DataTable dataTable = new DataTable();

            dataTable.TableName = "Dados Vendas - Produtos";

            dataTable.Columns.Add("Produto", typeof(int));
            dataTable.Columns.Add("Categoria", typeof(string));
            dataTable.Columns.Add("Data da Compra", typeof(DateTime));
            dataTable.Columns.Add("Valor Total", typeof(double));

            var dados = _estoqueInterface.ListagemRegistros();

            if (dados.Count > 0) // Verificar se há dados para evitar adicionar linhas vazias
            {
                foreach (var registro in dados) // Iterar sobre os registros e adicionar as linhas ao DataTable
                {
                    dataTable.Rows.Add(registro.ProdutoId, registro.CategoriaNome, registro.DataCompra, registro.Total);
                }
            }

            return dataTable;

        }


        [HttpPost]
        public async Task<IActionResult> BaixarEstoque(int id)
        {
            var produtoBaixado = await _estoqueInterface.CriarRegistro(id);

            TempData["MensagemSucesso"] = "Compra realizada com sucesso!";

            return RedirectToAction("Index", "Home");
        }
    }
}
