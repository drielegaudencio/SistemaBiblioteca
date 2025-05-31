using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    // Esta classe atua como a entidade de junção para o relacionamento M-M com informação
    // entre Usuario e Plano, e também tem relações com Funcionario.
    public class Assinatura
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O usuário da assinatura é obrigatório.")]
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public  Usuario? Usuario { get; set; } // Propriedade de navegação para o Usuário

        [Required(ErrorMessage = "O plano da assinatura é obrigatório.")]
        public int PlanoId { get; set; }
        [ForeignKey("PlanoId")]
        public Plano? Plano { get; set; } // Propriedade de navegação para o Plano

        [Required(ErrorMessage = "O funcionário responsável é obrigatório.")]
        public int FuncionarioResponsavelId { get; set; }
        [ForeignKey("FuncionarioResponsavelId")]
        public  Funcionario? FuncionarioResponsavel { get; set; } // Propriedade de navegação para o Funcionário

        [Required(ErrorMessage = "A data de início é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataInicio { get; set; }

        //[Required(ErrorMessage = "A data de fim é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime? DataFim { get; set; }

        /*[Required(ErrorMessage = "O valor cobrado é obrigatório.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 9999999999999999.99, ErrorMessage = "O valor cobrado deve ser um valor positivo.")]
        public decimal ValorCobrado { get; set; }*/

        [Required(ErrorMessage = "O status de ativação é obrigatório.")]
        public bool Ativa { get; set; } // Indica se a assinatura está ativa no momento
    }
}
