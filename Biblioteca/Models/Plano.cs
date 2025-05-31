using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Plano
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do plano é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do plano não pode exceder 100 caracteres.")]
        public required string Nome { get; set; } // Ex: "Plano 1 - Uso Local", "Plano 2 - Empréstimo Mensal"

        [StringLength(500, ErrorMessage = "A descrição do plano não pode exceder 500 caracteres.")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O valor mensal é obrigatório.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 9999999999999999.99, ErrorMessage = "O valor mensal deve ser um valor positivo.")]
        public decimal ValorMensal { get; set; }

        [Required(ErrorMessage = "O limite de empréstimos mensais é obrigatório.")]
        [Range(0, int.MaxValue, ErrorMessage = "O limite de empréstimos deve ser um valor não negativo.")]
        public int LimiteEmprestimosMensais { get; set; } // 0 para Plano 1, 1 para Plano 2

        [Required(ErrorMessage = "O prazo de devolução é obrigatório.")]
        [Range(0, int.MaxValue, ErrorMessage = "O prazo de devolução deve ser um valor não negativo.")]
        public int PrazoDevolucaoDias { get; set; } // Ex: 30 para Plano 2

        [Required(ErrorMessage = "A taxa de atraso diária é obrigatória.")]
        [Column(TypeName = "decimal(18,4)")] // Para armazenar percentuais como 0.01
        [Range(0, 1, ErrorMessage = "A taxa de atraso deve estar entre 0 e 1 (0% a 100%).")]
        public decimal TaxaAtrasoDiariaPercentual { get; set; } // Ex: 0.01 (1%)

        // Propriedade de navegação para as assinaturas associadas a este plano
        public ICollection<Assinatura>? Assinaturas { get; set; }
    }
}
