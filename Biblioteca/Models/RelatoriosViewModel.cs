using System.ComponentModel.DataAnnotations;
using Biblioteca.Models; // Certifique-se de que seus modelos estão neste namespace

namespace Biblioteca.ViewModels
{
    public class RelatoriosViewModel
    {
        // Relatório de Livros Mais Emprestados
        public List<LivroMaisEmprestadoViewModel> LivrosMaisEmprestados { get; set; } = new List<LivroMaisEmprestadoViewModel>();

        // Relatório de Usuários Mais Ativos (por número de empréstimos)
        public List<UsuarioMaisAtivoViewModel> UsuariosMaisAtivos { get; set; } = new List<UsuarioMaisAtivoViewModel>();

        // Relatório de Status dos Exemplares (Disponível/Emprestado)
        public List<ExemplarStatusViewModel> ExemplaresStatus { get; set; } = new List<ExemplarStatusViewModel>();

        // Relatório de Assinaturas Ativas/Inativas
        public List<AssinaturaResumoViewModel> AssinaturasAtivas { get; set; } = new List<AssinaturaResumoViewModel>();
        public List<AssinaturaResumoViewModel> AssinaturasInativas { get; set; } = new List<AssinaturaResumoViewModel>();
    }

    // Sub-ViewModel para Livros Mais Emprestados
    public class LivroMaisEmprestadoViewModel
    {
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int TotalEmprestimos { get; set; }
    }

    // Sub-ViewModel para Usuários Mais Ativos
    public class UsuarioMaisAtivoViewModel
    {
        public string NomeUsuario { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public int TotalEmprestimos { get; set; }
    }

    // Sub-ViewModel para Status de Exemplares
    public class ExemplarStatusViewModel
    {
        public string CodigoInventario { get; set; } = string.Empty;
        public string LivroTitulo { get; set; } = string.Empty;
        public bool Disponivel { get; set; }
        public int LivroId { get; set; } // Adicionado para possível navegação ou detalhe
    }

    // Sub-ViewModel para Resumo de Assinaturas
    public class AssinaturaResumoViewModel
    {
        public string NomeUsuario { get; set; } = string.Empty;
        public string PlanoNome { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DataFim { get; set; }
        
        public bool Ativa { get; set; }
    }
}
