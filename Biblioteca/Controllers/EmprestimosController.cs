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
    public class EmprestimosController : Controller
    {
        private readonly BibliotecaDbContext _context;

        public EmprestimosController(BibliotecaDbContext context)
        {
            _context = context;
        }

        // GET: Emprestimos
        public async Task<IActionResult> Index()
        {
            var bibliotecaDbContext = _context.Emprestimos.Include(e => e.Exemplar).Include(e => e.FuncionarioResponsavel).Include(e => e.Usuario);
            return View(await bibliotecaDbContext.ToListAsync());
        }

        // GET: Emprestimos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimos
                .Include(e => e.Exemplar)
                .Include(e => e.FuncionarioResponsavel)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        // GET: Emprestimos/Create
        public IActionResult Create()
        {
            var usuarioListItems = _context.Usuarios.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Nome
            }).ToList(); // Converte para List<SelectListItem>
            // Adiciona o item "Selecione" no início da lista
            usuarioListItems.Insert(0, new SelectListItem { Value = "", Text = "Selecione o Usuário" });
            ViewBag.UsuarioId = usuarioListItems;

            var exemplarListItems = _context.Exemplares.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Livro.Titulo.ToString()
            }).ToList(); // Converte para List<SelectListItem>
            // Adiciona o item "Selecione" no início da lista
            exemplarListItems.Insert(0, new SelectListItem { Value = "", Text = "Selecione o Livro" });
            ViewBag.ExemplarId = exemplarListItems;

            var funcionarioListItems = _context.Funcionarios.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Nome
            }).ToList(); // Converte para List<SelectListItem>
            // Adiciona o item "Selecione" no início da lista
            funcionarioListItems.Insert(0, new SelectListItem { Value = "", Text = "Selecione um Funcionário" });
            ViewBag.FuncionarioResponsavelId = funcionarioListItems;

            //ViewData["ExemplarId"] = new SelectList(_context.Exemplares, "Id", "CodigoInventario");
            //ViewData["FuncionarioResponsavelId"] = new SelectList(_context.Funcionarios, "Id", "Cpf");
            //ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Cpf");
            return View();
        }

        // POST: Emprestimos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioId,ExemplarId,FuncionarioResponsavelId,DataEmprestimo,DataDevolucaoPrevista,DataDevolucaoReal,MultaPaga,Devolvido")] Emprestimo emprestimo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emprestimo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var usuarioListItems = _context.Usuarios.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Nome
            }).ToList(); // Converte para List<SelectListItem>
            // Adiciona o item "Selecione" no início da lista
            usuarioListItems.Insert(0, new SelectListItem { Value = "", Text = "Selecione o Usuário" });
            ViewBag.UsuarioId = usuarioListItems;

            var exemplarListItems = _context.Exemplares.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Livro.Titulo.ToString()
            }).ToList(); // Converte para List<SelectListItem>
            // Adiciona o item "Selecione" no início da lista
            exemplarListItems.Insert(0, new SelectListItem { Value = "", Text = "Selecione o Livro" });
            ViewBag.ExemplarId = exemplarListItems;

            var funcionarioListItems = _context.Funcionarios.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Nome
            }).ToList(); // Converte para List<SelectListItem>
            // Adiciona o item "Selecione" no início da lista
            funcionarioListItems.Insert(0, new SelectListItem { Value = "", Text = "Selecione um Funcionário" });
            ViewBag.FuncionarioResponsavelId = funcionarioListItems;

            //ViewData["ExemplarId"] = new SelectList(_context.Exemplares, "Id", "CodigoInventario", emprestimo.ExemplarId);
            //ViewData["FuncionarioResponsavelId"] = new SelectList(_context.Funcionarios, "Id", "Cpf", emprestimo.FuncionarioResponsavelId);
            //ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Cpf", emprestimo.UsuarioId);
            return View(emprestimo);
        }

        // GET: Emprestimos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimos.FindAsync(id);
            if (emprestimo == null)
            {
                return NotFound();
            }
            var livroItems = _context.Exemplares.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = u.Livro.Titulo
            }).ToList(); // Converte para List<SelectListItem>
            // Adiciona o item "Selecione" no início da lista
            livroItems.Insert(0, new SelectListItem { Value = "", Text = "Selecione um Livro" });
            ViewBag.ExemplarId = livroItems;
            //ViewData["ExemplarId"] = new SelectList(_context.Exemplares, "Id", "CodigoInventario", emprestimo.ExemplarId);
            ViewData["FuncionarioResponsavelId"] = new SelectList(_context.Funcionarios, "Id", "Cpf", emprestimo.FuncionarioResponsavelId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Cpf", emprestimo.UsuarioId);
            return View(emprestimo);
        }

        // POST: Emprestimos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UsuarioId,ExemplarId,FuncionarioResponsavelId,DataEmprestimo,DataDevolucaoPrevista,DataDevolucaoReal,MultaPaga,Devolvido")] Emprestimo emprestimo)
        {
            if (id != emprestimo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emprestimo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmprestimoExists(emprestimo.Id))
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
            ViewData["ExemplarId"] = new SelectList(_context.Exemplares, "Id", "CodigoInventario", emprestimo.ExemplarId);
            ViewData["FuncionarioResponsavelId"] = new SelectList(_context.Funcionarios, "Id", "Cpf", emprestimo.FuncionarioResponsavelId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Cpf", emprestimo.UsuarioId);
            return View(emprestimo);
        }

        // GET: Emprestimos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprestimo = await _context.Emprestimos
                .Include(e => e.Exemplar)
                .Include(e => e.FuncionarioResponsavel)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        // POST: Emprestimos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emprestimo = await _context.Emprestimos.FindAsync(id);
            if (emprestimo != null)
            {
                _context.Emprestimos.Remove(emprestimo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmprestimoExists(int id)
        {
            return _context.Emprestimos.Any(e => e.Id == id);
        }
    }
}
