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
    public class UsuariosController : Controller
    {
        private readonly BibliotecaDbContext _context;

        public UsuariosController(BibliotecaDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var bibliotecaDbContext = _context.Usuarios.Include(u => u.AssinaturaAtual);
            return View(await bibliotecaDbContext.ToListAsync());
        }

        public async Task<IActionResult> List()
        {
            var bibliotecaDbContext = _context.Usuarios.Include(u => u.AssinaturaAtual);
            return View(await bibliotecaDbContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.AssinaturaAtual)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["AssinaturaAtualId"] = new SelectList(_context.Assinaturas, "Id", "Id");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssinaturaAtualId,Id,Nome,Cpf,Endereco,Telefone,Email")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            var assinaturaListItems = _context.Assinaturas.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Id.ToString()
            }).ToList(); // Converte para List<SelectListItem>
            // Adiciona o item "Selecione" no início da lista
            assinaturaListItems.Insert(0, new SelectListItem { Value = "", Text = "Selecione uma Assinatura" });
            ViewBag.AssinaturaAtualId = assinaturaListItems;
            //ViewData["AssinaturaAtualId"] = new SelectList(_context.Assinaturas, "Id", "Id", usuario.AssinaturaAtualId);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["AssinaturaAtualId"] = new SelectList(_context.Assinaturas, "Id", "Id", usuario.AssinaturaAtualId);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssinaturaAtualId,Id,Nome,Cpf,Endereco,Telefone,Email")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            ViewData["AssinaturaAtualId"] = new SelectList(_context.Assinaturas, "Id", "Id", usuario.AssinaturaAtualId);
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.AssinaturaAtual)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
