using Microsoft.EntityFrameworkCore;
using RestAPIFurb.Models;

namespace RestAPIFurb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Equipamento> Equipamentos { get; set; } = null!;
        public DbSet<Tipo> Tipos { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Nomenclatura padrão: tabela no plural (já é o nome do DbSet), classe no singular.
            modelBuilder.Entity<Equipamento>().ToTable("Equipamentos");
            modelBuilder.Entity<Tipo>().ToTable("Tipos");
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");

            // Dados iniciais (seed) para facilitar os testes da arguição
            modelBuilder.Entity<Tipo>().HasData(
                new Tipo { Id = 1, Nome = "Computador" },
                new Tipo { Id = 2, Nome = "audiovisual" },
                new Tipo { Id = 3, Nome = "Impressora" }
            );

            modelBuilder.Entity<Equipamento>().HasData(
                new Equipamento { Id = 1, Nome = "Notebook Dell", TipoId = 1 },
                new Equipamento { Id = 2, Nome = "Projetor Epson", TipoId = 2 },
                new Equipamento { Id = 3, Nome = "Notebook Lenovo", TipoId = 1 }
            );

            // Usuário de teste: login "admin" / senha "123456" (hash SHA256 gerado no seed abaixo)
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Login = "admin", SenhaHash = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92" }
            );
        }
    }
}
