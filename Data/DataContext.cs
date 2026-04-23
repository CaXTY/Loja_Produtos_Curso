using LojaProdutosCurso.Models;
using Microsoft.EntityFrameworkCore;

namespace LojaProdutosCurso.Data
{
    public class DataContext : DbContext
    {


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // Propriedades DbSet para cada entidade do modelo de dados
        public DbSet<ProdutoModel> Produtos { get; set; }
        public DbSet<CategoriaModel> Categorias { get; set; }
        public DbSet<ProdutosBaixadosModel> ProdutosBaixados { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<EnderecoModel> Enderecos { get; set; }


        // // Método para configurar o modelo de dados e definir as relações entre as entidades
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<CategoriaModel>().HasData(
                new CategoriaModel { Id = 1, Nome = "Tenis" },
                new CategoriaModel { Id = 2, Nome = "Botas" },
                new CategoriaModel { Id = 3, Nome = "Chinelos" },
                new CategoriaModel { Id = 4, Nome = "Sandalias" },
                new CategoriaModel { Id = 5, Nome = "Sapatos" }
       
            );

            base.OnModelCreating(modelBuilder);
        }

    }
}
