using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Genero
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do gênero é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome do gênero não pode exceder 50 caracteres.")]
        public required string Nome { get; set; }

        // Propriedade de navegação para o relacionamento M-M sem informação
        public ICollection<LivroGenero>? LivroGeneros { get; set; }
    }
}
