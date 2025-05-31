using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Funcionario : Pessoa
    {
        [Required(ErrorMessage = "O cargo é obrigatório.")]
        [StringLength(50, ErrorMessage = "O cargo não pode exceder 50 caracteres.")]
        public required string Cargo { get; set; }

        [Column(TypeName = "decimal(18,2)")] // Define o tipo de coluna para o banco de dados
        [Range(0, 9999999999999999.99, ErrorMessage = "O salário deve ser um valor positivo.")]
        public decimal Salario { get; set; }

        [Required(ErrorMessage ="A Matrícula é obrigatória.")]
        public required string Matricula { get; set; }

        // Propriedades de navegação para relacionamentos
        public ICollection<Assinatura>? AssinaturasRealizadas { get; set; } // Assinaturas que este funcionário registrou
        public ICollection<Emprestimo>? EmprestimosRealizados { get; set; } // Empréstimos que este funcionário registrou
    }
}
