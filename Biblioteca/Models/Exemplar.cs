using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Exemplar
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O livro associado é obrigatório.")]
        public int LivroId { get; set; }
        [ForeignKey("LivroId")]
        public Livro? Livro { get; set; } // Propriedade de navegação para o Livro (obra)

        [Required(ErrorMessage = "O código de inventário é obrigatório.")]
        [StringLength(50, ErrorMessage = "O código de inventário não pode exceder 50 caracteres.")]
        public required string CodigoInventario { get; set; } // Código único para este exemplar físico

        [Required(ErrorMessage = "O status de disponibilidade é obrigatório.")]
        public bool Disponivel { get; set; } // Indica se o exemplar está na estante ou emprestado

        // Histórico de empréstimos deste exemplar
        public ICollection<Emprestimo>? HistoricoEmprestimos { get; set; }
    }
}
