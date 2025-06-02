// Controllers/FuncionariosController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Data; // Usar o namespace correto do seu DbContext
using Biblioteca.Models; // Ajuste o namespace dos seus modelos
using Biblioteca.ViewModels; // Adicione este using
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Drawing.Text;

namespace Biblioteca.Controllers
{
    public class FuncionariosController : Controller
    {
        private readonly BibliotecaDbContext _context; // CORRIGIDO: Usando BibliotecaDbContext

        public FuncionariosController(BibliotecaDbContext context) // CORRIGIDO: Usando BibliotecaDbContext no construtor
        {
            _context = context;
        }

        // GET: Funcionarios
        /// <summary>
        /// Exibe a lista de todos os funcionários.
        /// </summary>
        /// <returns>A view com a lista de funcionários.</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Funcionarios.ToListAsync());
        }

        public async Task<IActionResult> List()
        {
            return View(await _context.Funcionarios.ToListAsync());
        }

        // GET: Funcionarios/Details/5
        /// <summary>
        /// Exibe os detalhes de um funcionário específico.
        /// </summary>
        /// <param name="id">O ID do funcionário.</param>
        /// <returns>A view com os detalhes do funcionário.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        // GET: Funcionarios/Create
        /// <summary>
        /// Exibe o formulário para criar um novo funcionário.
        /// </summary>
        /// <returns>A view do formulário de criação.</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Funcionarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cargo,Salario,Matricula,Id,Nome,Cpf,Endereco,Telefone,Email")] Funcionario funcionario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                 
                    _context.Add(funcionario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(Exception e){
                ModelState.AddModelError("","Não foi possivel inserir os dados");
            }
            
            return View(funcionario);
        }

        // GET: Funcionarios/Edit/5
        /// <summary>
        /// Exibe o formulário para editar um funcionário existente.
        /// </summary>
        /// <param name="id">O ID do funcionário a ser editado.</param>
        /// <returns>A view do formulário de edição.</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
            {
                return NotFound();
            }
            return View(funcionario);
        }

        // POST: Funcionarios/Edit/5
        /// <summary>
        /// Processa o envio do formulário para editar um funcionário.
        /// </summary>
        /// <param name="id">O ID do funcionário a ser editado.</param>
        /// <param name="funcionario">O objeto Funcionário com as informações atualizadas.</param>
        /// <returns>Redireciona para a lista de funcionários ou exibe o formulário com erros.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cpf,Endereco,Telefone,Email,Cargo,Salario,Matricula")] Funcionario funcionario)
        {
            if (id != funcionario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funcionario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncionarioExists(funcionario.Id))
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
            return View(funcionario);
        }

        // GET: Funcionarios/Delete/5
        /// <summary>
        /// Exibe a confirmação para deletar um funcionário.
        /// </summary>
        /// <param name="id">O ID do funcionário a ser deletado.</param>
        /// <returns>A view de confirmação de exclusão.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        // POST: Funcionarios/Delete/5
        /// <summary>
        /// Deleta o funcionário do banco de dados.
        /// </summary>
        /// <param name="id">O ID do funcionário a ser deletado.</param>
        /// <returns>Redireciona para a lista de funcionários.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario != null)
            {
                _context.Funcionarios.Remove(funcionario);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FuncionarioExists(int id)
        {
            return _context.Funcionarios.Any(e => e.Id == id);
        }

        // GET: Funcionarios/Search (página que o usuário acessa para buscar)
        /// <summary>
        /// Exibe a página de busca de funcionários.
        /// </summary>
        /// <returns>A view da página de busca.</returns>
        public IActionResult Search()
        {
            return View();
        }

        // GET: Funcionarios/SearchApi (endpoint que o JavaScript vai chamar para buscar dados)
        /// <summary>
        /// Endpoint de API para buscar detalhes de um funcionário por nome ou matrícula.
        /// Retorna informações pessoais do funcionário e todas as assinaturas que ele realizou.
        /// </summary>
        /// <param name="termoBusca">O nome ou matrícula do funcionário a ser buscado.</param>
        /// <returns>Um objeto JSON com os detalhes do funcionário e suas assinaturas realizadas.</returns>
        [HttpGet]
        public async Task<IActionResult> SearchApi(string termoBusca)
        {
            if (string.IsNullOrWhiteSpace(termoBusca))
            {
                return BadRequest(new { message = "Termo de busca não pode ser vazio." });
            }

            // Tenta encontrar o funcionário por matrícula exata ou por nome (contém, ignorando caixa)
            var funcionario = await _context.Funcionarios
                                            .Include(f => f.AssinaturasRealizadas) // CORRIGIDO: Usando AssinaturasRealizadas
                                                .ThenInclude(a => a.Plano) // Inclui o Plano da assinatura para exibir o nome do plano
                                            .Include(f => f.AssinaturasRealizadas) // Inclui novamente para ThenInclude o Usuário
                                                .ThenInclude(a => a.Usuario) // Inclui o Usuário que fez a assinatura
                                            .FirstOrDefaultAsync(f => f.Matricula == termoBusca || f.Nome.ToLower().Contains(termoBusca.ToLower()));

            if (funcionario == null)
            {
                return NotFound(new { message = "Funcionário não encontrado." });
            }

            // Mapear para o ViewModel simplificado
            var result = new FuncionarioSearchResultViewModel
            {
                Funcionario = new FuncionarioViewModel
                {
                    Id = funcionario.Id,
                    Nome = funcionario.Nome,
                    Matricula = funcionario.Matricula ?? "N/A", // Adicionado tratamento para null
                    Cargo = funcionario.Cargo ?? "N/A", // Adicionado o Cargo
                    Email = funcionario.Email ?? "N/A",
                    Telefone = funcionario.Telefone ?? "N/A",
                    Cpf = funcionario.Cpf ?? "N/A", // Incluindo CPF
                    Endereco = funcionario.Endereco ?? "N/A", // Incluindo Endereco
                    Salario = funcionario.Salario // Incluindo Salario
                },
                // CORRIGIDO: Mapeando de funcionario.AssinaturasRealizadas
                AssinaturasRealizadas = funcionario.AssinaturasRealizadas?
                    .OrderByDescending(a => a.DataInicio) // Ordena as assinaturas pela data mais recente
                    .Select(a => new AssinaturaViewModel
                    {
                        Id = a.Id,
                        NomeUsuario = a.Usuario?.Nome ?? "Usuário Desconhecido", // Nome do usuário da assinatura
                        TipoPlano = a.Plano?.Nome ?? "Plano Desconhecido", // Nome do plano
                        DataInicio = a.DataInicio.ToString("dd/MM/yyyy"),
                        DataFim = a.DataFim?.ToString("dd/MM/yyyy") ?? "Indefinida",
                        Ativa = a.Ativa,
                        // ValorCobrado REMOVIDO pois está comentado no seu modelo Assinatura.cs
                        // Se você descomentar ValorCobrado em Assinatura.cs, você pode adicioná-lo aqui.
                        // ValorCobrado = a.ValorCobrado
                    }).ToList()
            };

            return Ok(result);
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
using Biblioteca.ViewModels;

namespace Biblioteca.Controllers
{
    public class FuncionariosController : Controller
    {
        private readonly BibliotecaDbContext _context;


        public FuncionariosController(BibliotecaDbContext context)
        {
            _context = context;
        }

        // GET: Funcionarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Funcionarios.ToListAsync());
        }

        public async Task<IActionResult> List()
        {
            return View(await _context.Funcionarios.ToListAsync());
        }

        // GET: Funcionarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        // GET: Funcionarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Funcionarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cargo,Salario,Matricula,Id,Nome,Cpf,Endereco,Telefone,Email")] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(funcionario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(funcionario);
        }

        // GET: Funcionarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
            {
                return NotFound();
            }
            return View(funcionario);
        }

        // POST: Funcionarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cargo,Salario,Matricula,Id,Nome,Cpf,Endereco,Telefone,Email")] Funcionario funcionario)
        {
            if (id != funcionario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funcionario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncionarioExists(funcionario.Id))
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
            return View(funcionario);
        }

        // GET: Funcionarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        // POST: Funcionarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario != null)
            {
                _context.Funcionarios.Remove(funcionario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FuncionarioExists(int id)
        {
            return _context.Funcionarios.Any(e => e.Id == id);
        }

        public IActionResult Search()
        {
            return View();
        }

    }
}



*/