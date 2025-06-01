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
    public class AssinaturasController : Controller
    {
        private readonly BibliotecaDbContext _context;

        public AssinaturasController(BibliotecaDbContext context)
        {
            _context = context;
        }

        // GET: Assinaturas
        public async Task<IActionResult> Index()
        {
            var bibliotecaDbContext = _context.Assinaturas.Include(a => a.FuncionarioResponsavel).Include(a => a.Plano).Include(a => a.Usuario);
            return View(await bibliotecaDbContext.ToListAsync());
        }

        // GET: Assinaturas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assinatura = await _context.Assinaturas
                .Include(a => a.FuncionarioResponsavel)
                .Include(a => a.Plano)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assinatura == null)
            {
                return NotFound();
            }

            return View(assinatura);
        }

        // GET: Assinaturas/Create
        public IActionResult Create()
        {
            var usuarioListItems = _context.Usuarios.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Nome
            }).ToList(); // Converte para List<SelectListItem>
            // Adiciona o item "Selecione" no início da lista
            usuarioListItems.Insert(0, new SelectListItem { Value = "", Text = "Selecione um Usuário" });
            ViewBag.UsuarioId = usuarioListItems;

            var funcionarioListItems = _context.Funcionarios.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Nome
            }).ToList(); // Converte para List<SelectListItem>
            // Adiciona o item "Selecione" no início da lista
            funcionarioListItems.Insert(0, new SelectListItem { Value = "", Text = "Selecione um Funcionário" });
            ViewBag.FuncionarioResponsavelId = funcionarioListItems;


            var planoListItems = _context.Planos.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Nome
            }).ToList(); // Converte para List<SelectListItem>
            // Adiciona o item "Selecione" no início da lista
            planoListItems.Insert(0, new SelectListItem { Value = "", Text = "Selecione um Plano" });
            ViewBag.PlanoId = planoListItems;
            //ViewData["FuncionarioResponsavelId"] = new SelectList(_context.Funcionarios, "Id", "Cpf");
            //ViewData["PlanoId"] = new SelectList(_context.Planos, "Id", "Nome");
            // ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Cpf");
            
            return View();
        }

        // POST: Assinaturas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioId,PlanoId,FuncionarioResponsavelId,DataInicio,DataFim,Ativa")] Assinatura assinatura)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assinatura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FuncionarioResponsavelId"] = new SelectList(_context.Funcionarios, "Id", "Cpf", assinatura.FuncionarioResponsavelId);
            ViewData["PlanoId"] = new SelectList(_context.Planos, "Id", "Nome", assinatura.PlanoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Cpf", assinatura.UsuarioId);
            return View(assinatura);
        }

        // GET: Assinaturas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assinatura = await _context.Assinaturas.FindAsync(id);
            if (assinatura == null)
            {
                return NotFound();
            }
            ViewData["FuncionarioResponsavelId"] = new SelectList(_context.Funcionarios, "Id", "Cpf", assinatura.FuncionarioResponsavelId);
            ViewData["PlanoId"] = new SelectList(_context.Planos, "Id", "Nome", assinatura.PlanoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Cpf", assinatura.UsuarioId);
            return View(assinatura);
        }

        // POST: Assinaturas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UsuarioId,PlanoId,FuncionarioResponsavelId,DataInicio,DataFim,Ativa")] Assinatura assinatura)
        {
            if (id != assinatura.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assinatura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssinaturaExists(assinatura.Id))
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
            ViewData["FuncionarioResponsavelId"] = new SelectList(_context.Funcionarios, "Id", "Cpf", assinatura.FuncionarioResponsavelId);
            ViewData["PlanoId"] = new SelectList(_context.Planos, "Id", "Nome", assinatura.PlanoId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Cpf", assinatura.UsuarioId);
            return View(assinatura);
        }

        // GET: Assinaturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assinatura = await _context.Assinaturas
                .Include(a => a.FuncionarioResponsavel)
                .Include(a => a.Plano)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assinatura == null)
            {
                return NotFound();
            }

            return View(assinatura);
        }

        // POST: Assinaturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assinatura = await _context.Assinaturas.FindAsync(id);
            if (assinatura != null)
            {
                _context.Assinaturas.Remove(assinatura);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssinaturaExists(int id)
        {
            return _context.Assinaturas.Any(e => e.Id == id);
        }
    }
}
