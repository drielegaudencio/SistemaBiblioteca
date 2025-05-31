using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Emprestimo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O usuário do empréstimo é obrigatório.")]
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public required Usuario Usuario { get; set; } // Propriedade de navegação para o Usuário

        [Required(ErrorMessage = "O exemplar emprestado é obrigatório.")]
        public int ExemplarId { get; set; }
        [ForeignKey("ExemplarId")]
        public required Exemplar Exemplar { get; set; } // Propriedade de navegação para o Exemplar

        [Required(ErrorMessage = "O funcionário responsável é obrigatório.")]
        public int FuncionarioResponsavelId { get; set; }
        [ForeignKey("FuncionarioResponsavelId")]
        public required Funcionario FuncionarioResponsavel { get; set; } // Propriedade de navegação para o Funcionário

        [Required(ErrorMessage = "A data do empréstimo é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataEmprestimo { get; set; }

        [Required(ErrorMessage = "A data de devolução prevista é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataDevolucaoPrevista { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataDevolucaoReal { get; set; } // Nullable, preenchido na devolução

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 9999999999999999.99, ErrorMessage = "A multa deve ser um valor não negativo.")]
        public decimal MultaPaga { get; set; } // Valor da multa, se houver

        [Required(ErrorMessage = "O status de devolução é obrigatório.")]
        public bool Devolvido { get; set; } // Indica se o exemplar já foi devolvido
    }
}
