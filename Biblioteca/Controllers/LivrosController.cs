// Controllers/LivrosController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Data;
using Biblioteca.Models;
using Biblioteca.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic; // Para List
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Biblioteca.ViewModels; // Para Regex.IsMatch

namespace Biblioteca.Controllers
{
    public class LivrosController : Controller
    {
        private readonly BibliotecaDbContext _context;
        private readonly OpenLibraryService _openLibraryService;

        public LivrosController(BibliotecaDbContext context, OpenLibraryService openLibraryService)
        {
            _context = context;
            _openLibraryService = openLibraryService;
        }

        // GET: Livros
        // Exibe a lista de todos os livros
        public async Task<IActionResult> Index()
        {
            // Carrega os livros e inclui os LivroGeneros e Generos associados
            var livros = await _context.Livros
                                       .Include(l => l.LivroGeneros)
                                           .ThenInclude(lg => lg.Genero)
                                       .ToListAsync();
            return View(livros);
        }
        public async Task<IActionResult> List()
        {
            // Carrega os livros e inclui os LivroGeneros e Generos associados
            var livros = await _context.Livros
                                       .Include(l => l.LivroGeneros)
                                           .ThenInclude(lg => lg.Genero)
                                       .ToListAsync();
            return View(livros);
        }

        // GET: Livros/Details/5
        // Exibe os detalhes de um livro específico
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                                      .Include(l => l.Exemplares) // Inclui os exemplares do livro
                                      .Include(l => l.LivroGeneros)
                                          .ThenInclude(lg => lg.Genero)
                                      .FirstOrDefaultAsync(m => m.Id == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // GET: Livros/Create
        // Exibe o formulário para criar um novo livro
        public IActionResult Create()
        {
            /*var viewModel = new LivroViewModel();
            // Popula a lista de gêneros disponíveis para a View
            viewModel.AvailableGenres = await _context.Generos
                                                    .Select(g => new SelectListItem
                                                    {
                                                        Value = g.Id.ToString(),
                                                        Text = g.Nome
                                                    })
                                                    .ToListAsync();
            return View(viewModel);
*/
            var generoListItems = _context.Generos.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Nome
            }).ToList(); // Converte para List<SelectListItem>
            // Adiciona o item "Selecione" no início da lista
            generoListItems.Insert(0, new SelectListItem { Value = "", Text = "Selecione um Gênero" });
            ViewBag.GeneroPrincipal = generoListItems;
            return View();
        }

        // POST: Livros/Create
        // Processa o envio do formulário para criar um novo livro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LivroViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var livro = new Livro
                {
                    Titulo = viewModel.Titulo,
                    Autor = viewModel.Autor,
                    Isbn = viewModel.Isbn,
                    AnoPublicacao = viewModel.AnoPublicacao,
                    NumeroPaginas = viewModel.NumeroPaginas,
                    GeneroPrincipal = viewModel.GeneroPrincipal, // Mantenha se ainda usar um 'principal'
                    // Outras propriedades do Livro que não sejam coleções de navegação
                };

               /* var generoListItems = _context.Generos.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Nome
                }).ToList(); // Converte para List<SelectListItem>
                             // Adiciona o item "Selecione" no início da lista
                generoListItems.Insert(0, new SelectListItem { Value = "", Text = "Selecione um Gênero" });
                ViewBag.GeneroPrincipal = generoListItems;
*/
                // A lógica de preenchimento automático do formulário pela API agora será mais controlada pelo JS na View.
                // Esta parte do código ainda pode ser útil para uma validação final ou para preencher se o ISBN for digitado manualmente.

                if (!string.IsNullOrWhiteSpace(livro.Isbn) && string.IsNullOrWhiteSpace(livro.Titulo))
                {
                    var openLibraryData = await _openLibraryService.GetBookByIsbn(livro.Isbn);
                    if (openLibraryData != null)
                    {
                        if (string.IsNullOrWhiteSpace(livro.Titulo)) livro.Titulo = openLibraryData.Title;
                        if (string.IsNullOrWhiteSpace(livro.Autor) && openLibraryData.Authors != null && openLibraryData.Authors.Any())
                        {
                            livro.Autor = string.Join(", ", openLibraryData.Authors);
                        }
                        if (!livro.NumeroPaginas.HasValue && openLibraryData.NumberOfPages.HasValue)
                        {
                            livro.NumeroPaginas = openLibraryData.NumberOfPages;
                        }
                        if (!livro.AnoPublicacao.HasValue && openLibraryData.PublishYear.HasValue)
                        {
                            livro.AnoPublicacao = openLibraryData.PublishYear;
                        }
                        if (string.IsNullOrWhiteSpace(livro.GeneroPrincipal) && openLibraryData.Subjects != null && openLibraryData.Subjects.Any())
                        {
                            livro.GeneroPrincipal = openLibraryData.Subjects.FirstOrDefault();
                        }
                    }
                }
                _context.Add(livro);
                await _context.SaveChangesAsync();
                // Agora, salve as relações LivroGenero
                if (viewModel.SelectedGenreIds != null && viewModel.SelectedGenreIds.Any())
                {
                    foreach (var generoId in viewModel.SelectedGenreIds)
                    {
                        var livroGenero = new LivroGenero
                        {
                            LivroId = livro.Id, // Usa o ID do livro recém-salvo
                            GeneroId = generoId
                        };
                        _context.LivroGeneros.Add(livroGenero);
                    }
                    await _context.SaveChangesAsync(); // Salva as relações
                }

                return RedirectToAction(nameof(Index));
            }
            // Se o ModelState não for válido, repopula a lista de gêneros para a View
            viewModel.AvailableGenres = await _context.Generos
                                                    .Select(g => new SelectListItem
                                                    {
                                                        Value = g.Id.ToString(),
                                                        Text = g.Nome
                                                    })
                                                    .ToListAsync();
            return View(viewModel);
        }

        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                                    .Include(l => l.LivroGeneros) // Inclui as relações existentes
                                    .FirstOrDefaultAsync(m => m.Id == id);
            if (livro == null)
            {
                return NotFound();
            }

            var viewModel = new LivroViewModel
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Autor = livro.Autor,
                Isbn = livro.Isbn,
                AnoPublicacao = (int)livro.AnoPublicacao,
                NumeroPaginas = (int)livro.NumeroPaginas,
                GeneroPrincipal = livro.GeneroPrincipal,
                SelectedGenreIds = livro.LivroGeneros?.Select(lg => lg.GeneroId).ToList() // Popula os gêneros selecionados
            };

            // Popula a lista de todos os gêneros disponíveis
            viewModel.AvailableGenres = await _context.Generos
                                                    .Select(g => new SelectListItem
                                                    {
                                                        Value = g.Id.ToString(),
                                                        Text = g.Nome,
                                                        Selected = viewModel.SelectedGenreIds != null && viewModel.SelectedGenreIds.Contains(g.Id) // Marca os já selecionados
                                                    })
                                                    .ToListAsync();
            return View(viewModel);
        }

        // POST: Livros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LivroViewModel viewModel) // Recebe o ViewModel
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var livro = await _context.Livros
                                        .Include(l => l.LivroGeneros)
                                        .FirstOrDefaultAsync(l => l.Id == id);

                if (livro == null)
                {
                    return NotFound();
                }

                // Atualiza as propriedades do livro
                livro.Titulo = viewModel.Titulo;
                livro.Autor = viewModel.Autor;
                livro.Isbn = viewModel.Isbn;
                livro.AnoPublicacao = viewModel.AnoPublicacao;
                livro.NumeroPaginas = viewModel.NumeroPaginas;
                livro.GeneroPrincipal = viewModel.GeneroPrincipal;

                // Atualiza as relações LivroGenero
                // 1. Remove as relações antigas
                _context.LivroGeneros.RemoveRange(livro.LivroGeneros);

                // 2. Adiciona as novas relações
                if (viewModel.SelectedGenreIds != null && viewModel.SelectedGenreIds.Any())
                {
                    foreach (var generoId in viewModel.SelectedGenreIds)
                    {
                        livro.LivroGeneros.Add(new LivroGenero { LivroId = livro.Id, GeneroId = generoId });
                    }
                }

                try
                {
                    _context.Update(livro); // Atualiza o livro e as relações em cascata
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivroExists(livro.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Se o ModelState não for válido, repopula a lista de gêneros para a View
            viewModel.AvailableGenres = await _context.Generos
                                                    .Select(g => new SelectListItem
                                                    {
                                                        Value = g.Id.ToString(),
                                                        Text = g.Nome,
                                                        Selected = viewModel.SelectedGenreIds != null && viewModel.SelectedGenreIds.Contains(g.Id)
                                                    })
                                                    .ToListAsync();
            return View(viewModel);
        }*/
    


        // GET: Livros/Edit/5
        // Exibe o formulário para editar um livro existente
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var generoListItems = _context.Generos.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Nome
            }).ToList();
            ViewBag.GeneroPrincipal = generoListItems;
            var livro = await _context.Livros.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }
            return View(livro);
        }

        // POST: Livros/Edit/5
        // Processa o envio do formulário para editar um livro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Autor,GeneroPrincipal,AnoPublicacao,Isbn,NumeroPaginas")] Livro livro)
        {
            if (id != livro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivroExists(livro.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(livro);
        }

// GET: Livros/Delete/5
// Exibe a confirmação para deletar um livro
public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // POST: Livros/Delete/5
        // Deleta o livro do banco de dados
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro != null)
            {
                _context.Livros.Remove(livro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LivroExists(int id)
        {
            return _context.Livros.Any(e => e.Id == id);
        }

        /// <summary>
        /// Busca livros na Open Library API por título ou ISBN.
        /// </summary>
        /// <param name="searchTerm">O termo de busca (título ou ISBN).</param>
        /// <returns>Uma lista de objetos OpenLibraryBookData em formato JSON.</returns>
        [HttpGet]
        public async Task<IActionResult> SearchOpenLibrary([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest(new { message = "O termo de busca não pode ser vazio." });
            }

            List<OpenLibraryBookData> results = new List<OpenLibraryBookData>();

            // Verifica se o termo de busca parece um ISBN (10 ou 13 dígitos, com ou sem hifens)
            // Regex para ISBN: ^(?:ISBN(?:-13)?:?)(?=[0-9]{13}$)([0-9]{3}-){2}[0-9]{3}[0-9X]$|^[0-9]{10}$
            // Simplificado para apenas números para este exemplo
            bool isIsbn = Regex.IsMatch(searchTerm, @"^\d{10}$") || Regex.IsMatch(searchTerm, @"^\d{13}$");

            if (isIsbn)
            {
                var book = await _openLibraryService.GetBookByIsbn(searchTerm);
                if (book != null)
                {
                    results.Add(book);
                }
            }
            else // Busca por título
            {
                var titleSearchResults = await _openLibraryService.SearchBooksByTitle(searchTerm);
                if (titleSearchResults != null && titleSearchResults.Any())
                {
                    results.AddRange(titleSearchResults);
                }
            }

            if (!results.Any())
            {
                return NotFound(new { message = "Nenhum livro encontrado para o termo fornecido na Open Library." });
            }

            return Ok(results);
        }
    }
}
/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Data;
using Biblioteca.Models;

namespace Biblioteca.Controllers
{
    public class LivrosController : Controller
    {
        private readonly BibliotecaDbContext _context;

        public LivrosController(BibliotecaDbContext context)
        {
            _context = context;
        }

        // GET: Livros
        public async Task<IActionResult> Index()
        {
            return View(await _context.Livros.ToListAsync());
        }

        // GET: Livros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // GET: Livros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Livros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Autor,GeneroPrincipal,AnoPublicacao,Isbn,NumeroPaginas")] Livro livro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(livro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(livro);
        }

        // GET: Livros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros.FindAsync(id);
            if (livro == null)
            {
                return NotFound();
            }
            return View(livro);
        }

        // POST: Livros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Autor,GeneroPrincipal,AnoPublicacao,Isbn,NumeroPaginas")] Livro livro)
        {
            if (id != livro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivroExists(livro.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(livro);
        }

        // GET: Livros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livro = await _context.Livros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livro == null)
            {
                return NotFound();
            }

            return View(livro);
        }

        // POST: Livros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro != null)
            {
                _context.Livros.Remove(livro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LivroExists(int id)
        {
            return _context.Livros.Any(e => e.Id == id);
        }
    }
}
*/