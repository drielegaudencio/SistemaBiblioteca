using System;
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
    public class LivroGenerosController : Controller
    {
        private readonly BibliotecaDbContext _context;

        public LivroGenerosController(BibliotecaDbContext context)
        {
            _context = context;
        }

        // GET: LivroGeneros
        public async Task<IActionResult> Index()
        {
            var bibliotecaDbContext = _context.LivroGeneros.Include(l => l.Genero).Include(l => l.Livro);
            return View(await bibliotecaDbContext.ToListAsync());
        }

        // GET: LivroGeneros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livroGenero = await _context.LivroGeneros
                .Include(l => l.Genero)
                .Include(l => l.Livro)
                .FirstOrDefaultAsync(m => m.LivroId == id);
            if (livroGenero == null)
            {
                return NotFound();
            }

            return View(livroGenero);
        }

        // GET: LivroGeneros/Create
        public IActionResult Create()
        {
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nome");
            ViewData["LivroId"] = new SelectList(_context.Livros, "Id", "Titulo");
            return View();
        }

        // POST: LivroGeneros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LivroId,GeneroId")] LivroGenero livroGenero)
        {
            if (ModelState.IsValid)
            {
                _context.Add(livroGenero);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nome", livroGenero.GeneroId);
            ViewData["LivroId"] = new SelectList(_context.Livros, "Id", "Titulo", livroGenero.LivroId);
            return View(livroGenero);
        }

        // GET: LivroGeneros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livroGenero = await _context.LivroGeneros.FindAsync(id);
            if (livroGenero == null)
            {
                return NotFound();
            }
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nome", livroGenero.GeneroId);
            ViewData["LivroId"] = new SelectList(_context.Livros, "Id", "Titulo", livroGenero.LivroId);
            return View(livroGenero);
        }

        // POST: LivroGeneros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LivroId,GeneroId")] LivroGenero livroGenero)
        {
            if (id != livroGenero.LivroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livroGenero);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivroGeneroExists(livroGenero.LivroId))
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
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nome", livroGenero.GeneroId);
            ViewData["LivroId"] = new SelectList(_context.Livros, "Id", "Titulo", livroGenero.LivroId);
            return View(livroGenero);
        }

        // GET: LivroGeneros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livroGenero = await _context.LivroGeneros
                .Include(l => l.Genero)
                .Include(l => l.Livro)
                .FirstOrDefaultAsync(m => m.LivroId == id);
            if (livroGenero == null)
            {
                return NotFound();
            }

            return View(livroGenero);
        }

        // POST: LivroGeneros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livroGenero = await _context.LivroGeneros.FindAsync(id);
            if (livroGenero != null)
            {
                _context.LivroGeneros.Remove(livroGenero);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LivroGeneroExists(int id)
        {
            return _context.LivroGeneros.Any(e => e.LivroId == id);
        }
    }
}
