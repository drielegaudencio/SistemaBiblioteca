using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Data
{
    public class BibliotecaDbContext : DbContext
    {
        public  BibliotecaDbContext(DbContextOptions<BibliotecaDbContext>options) : base(options)
        {
        }
        // DbSets para cada entidade que você deseja mapear para uma tabela no banco de dados
        public DbSet<Pessoa> Pessoas { get; set; } // Para gerenciar a hierarquia de herança
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Plano> Planos { get; set; }
        public DbSet<Assinatura> Assinaturas { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Exemplar> Exemplares { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<LivroGenero> LivroGeneros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da herança TPH (Table Per Hierarchy) para Pessoa
            // Uma única tabela 'Pessoas' conterá todos os Funcionarios e Usuarios,
            // com uma coluna 'Discriminator' para diferenciar os tipos.
            modelBuilder.Entity<Pessoa>()
                .HasDiscriminator<string>("TipoPessoa")
                .HasValue<Funcionario>("Funcionario")
                .HasValue<Usuario>("Usuario");

            // Relacionamento Muitos-para-Muitos com informação da relação: Usuario <-> Assinatura <-> Plano
            // Assinatura é a entidade de junção que contém informações adicionais (DataInicio, DataFim, etc.)
            modelBuilder.Entity<Assinatura>()
                .HasOne(a => a.Usuario)
                .WithMany(u => u.HistoricoAssinaturas) // Um usuário pode ter várias assinaturas ao longo do tempo
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata acidental

            modelBuilder.Entity<Assinatura>()
                .HasOne(a => a.Plano)
                .WithMany(p => p.Assinaturas)
                .HasForeignKey(a => a.PlanoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Assinatura>()
                .HasOne(a => a.FuncionarioResponsavel)
                .WithMany(f => f.AssinaturasRealizadas)
                .HasForeignKey(a => a.FuncionarioResponsavelId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Um-para-Um opcional: Usuario -> AssinaturaAtual
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.AssinaturaAtual)
                .WithOne() // Não há propriedade de navegação reversa na Assinatura para 'UsuarioAtual'
                .HasForeignKey<Usuario>(u => u.AssinaturaAtualId)
                .IsRequired(false); // Opcional, um usuário pode não ter assinatura ativa

            // Relacionamento Um-para-Muitos: Livro <-> Exemplar
            modelBuilder.Entity<Exemplar>()
                .HasOne(e => e.Livro)
                .WithMany(l => l.Exemplares)
                .HasForeignKey(e => e.LivroId)
                .OnDelete(DeleteBehavior.Cascade); // Se um livro for excluído, seus exemplares também são

            // Relacionamento Um-para-Muitos: Exemplar <-> Emprestimo
            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Exemplar)
                .WithMany(ex => ex.HistoricoEmprestimos)
                .HasForeignKey(e => e.ExemplarId)
                .OnDelete(DeleteBehavior.Restrict); // Não exclui exemplar se houver empréstimos

            // Relacionamento Um-para-Muitos: Usuario <-> Emprestimo
            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Usuario)
                .WithMany(u => u.Emprestimos)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Um-para-Muitos: Funcionario <-> Emprestimo
            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.FuncionarioResponsavel)
                .WithMany(f => f.EmprestimosRealizados)
                .HasForeignKey(e => e.FuncionarioResponsavelId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento Muitos-para-Muitos sem informação da relação: Livro <-> Genero
            // Usando uma entidade de junção explícita (LivroGenero) para melhor controle
            modelBuilder.Entity<LivroGenero>()
                .HasKey(lg => new { lg.LivroId, lg.GeneroId }); // Chave composta

            modelBuilder.Entity<LivroGenero>()
                .HasOne(lg => lg.Livro)
                .WithMany(l => l.LivroGeneros)
                .HasForeignKey(lg => lg.LivroId);

            modelBuilder.Entity<LivroGenero>()
                .HasOne(lg => lg.Genero)
                .WithMany(g => g.LivroGeneros)
                .HasForeignKey(lg => lg.GeneroId);
        }
    }
}
