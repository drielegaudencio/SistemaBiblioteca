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
    public class ExemplarsController : Controller
    {
        private readonly BibliotecaDbContext _context;

        public ExemplarsController(BibliotecaDbContext context)
        {
            _context = context;
        }

        // GET: Exemplars
        public async Task<IActionResult> Index()
        {
            var bibliotecaDbContext = _context.Exemplares.Include(e => e.Livro);
            return View(await bibliotecaDbContext.ToListAsync());
        }

        // GET: Exemplars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exemplar = await _context.Exemplares
                .Include(e => e.Livro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exemplar == null)
            {
                return NotFound();
            }

            return View(exemplar);
        }

        // GET: Exemplars/Create
        public IActionResult Create()
        {
            ViewData["LivroId"] = new SelectList(_context.Livros, "Id", "Titulo");
            return View();
        }

        // POST: Exemplars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LivroId,CodigoInventario,Disponivel")] Exemplar exemplar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exemplar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LivroId"] = new SelectList(_context.Livros, "Id", "Titulo", exemplar.LivroId);
            return View(exemplar);
        }

        // GET: Exemplars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exemplar = await _context.Exemplares.FindAsync(id);
            if (exemplar == null)
            {
                return NotFound();
            }
            ViewData["LivroId"] = new SelectList(_context.Livros, "Id", "Titulo", exemplar.LivroId);
            return View(exemplar);
        }

        // POST: Exemplars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LivroId,CodigoInventario,Disponivel")] Exemplar exemplar)
        {
            if (id != exemplar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exemplar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExemplarExists(exemplar.Id))
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
            ViewData["LivroId"] = new SelectList(_context.Livros, "Id", "Titulo", exemplar.LivroId);
            return View(exemplar);
        }

        // GET: Exemplars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exemplar = await _context.Exemplares
                .Include(e => e.Livro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exemplar == null)
            {
                return NotFound();
            }

            return View(exemplar);
        }

        // POST: Exemplars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exemplar = await _context.Exemplares.FindAsync(id);
            if (exemplar != null)
            {
                _context.Exemplares.Remove(exemplar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExemplarExists(int id)
        {
            return _context.Exemplares.Any(e => e.Id == id);
        }
    }
}
