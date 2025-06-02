using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Data; // Seu DbContext
using Biblioteca.ViewModels; // Seus ViewModels
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly BibliotecaDbContext _context;

        public RelatoriosController(BibliotecaDbContext context)
        {
            _context = context;
        }

        // GET: Relatorios
        public async Task<IActionResult> Index()
        {
            var viewModel = new RelatoriosViewModel();

            // 1. Relatório de Livros Mais Emprestados (Top 10)
            viewModel.LivrosMaisEmprestados = await _context.Emprestimos
                .Where(e => e.DataDevolucaoReal != null) // Considere apenas empréstimos finalizados ou todos ativos
                .GroupBy(e => e.Exemplar.LivroId)
                .Select(g => new LivroMaisEmprestadoViewModel
                {
                    Titulo = g.First().Exemplar.Livro.Titulo,
                    Autor = g.First().Exemplar.Livro.Autor,
                    TotalEmprestimos = g.Count()
                })
                .OrderByDescending(x => x.TotalEmprestimos)
                .Take(10)
                .ToListAsync();

            // 2. Relatório de Usuários Mais Ativos (Top 10)
            viewModel.UsuariosMaisAtivos = await _context.Emprestimos
                .GroupBy(e => e.UsuarioId)
                .Select(g => new UsuarioMaisAtivoViewModel
                {
                    NomeUsuario = g.First().Usuario.Nome,
                    Cpf = g.First().Usuario.Cpf,
                    TotalEmprestimos = g.Count()
                })
                .OrderByDescending(x => x.TotalEmprestimos)
                .Take(10)
                .ToListAsync();

            // 3. Relatório de Status dos Exemplares
            viewModel.ExemplaresStatus = await _context.Exemplares
                .Include(e => e.Livro) // Garante que o título do livro seja carregado
                .Select(e => new ExemplarStatusViewModel
                {
                    CodigoInventario = e.CodigoInventario,
                    LivroTitulo = e.Livro.Titulo,
                    Disponivel = e.Disponivel,
                    LivroId = e.LivroId
                })
                .OrderBy(e => e.LivroTitulo)
                .ThenBy(e => e.CodigoInventario)
                .ToListAsync();

            // 4. Relatório de Assinaturas Ativas/Inativas
            viewModel.AssinaturasAtivas = await _context.Assinaturas
                .Include(a => a.Usuario)
                .Include(a => a.Plano)
                .Where(a => a.Ativa)
                .Select(a => new AssinaturaResumoViewModel
                {
                    NomeUsuario = a.Usuario.Nome,
                    PlanoNome = a.Plano.Nome,
                    DataInicio = a.DataInicio,
                    DataFim = a.DataFim,
                    Ativa = a.Ativa
                })
                .OrderBy(a => a.NomeUsuario)
                .ToListAsync();

            viewModel.AssinaturasInativas = await _context.Assinaturas
                .Include(a => a.Usuario)
                .Include(a => a.Plano)
                .Where(a => !a.Ativa)
                .Select(a => new AssinaturaResumoViewModel
                {
                    NomeUsuario = a.Usuario.Nome,
                    PlanoNome = a.Plano.Nome,
                    DataInicio = a.DataInicio,
                    DataFim = a.DataFim,
                    Ativa = a.Ativa
                })
                .OrderBy(a => a.NomeUsuario)
                .ToListAsync();


            return View(viewModel);
        }
    }
}
