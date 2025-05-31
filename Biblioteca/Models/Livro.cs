using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Livro
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(200, ErrorMessage = "O título não pode exceder 200 caracteres.")]
        public required string Titulo { get; set; }

        [StringLength(100, ErrorMessage = "O autor não pode exceder 100 caracteres.")]
        public string? Autor { get; set; } // Pode ser nulo, ou preenchido pela API externa

        [StringLength(50, ErrorMessage = "O gênero não pode exceder 50 caracteres.")]
        public string? GeneroPrincipal { get; set; } // Gênero principal, se houver

        [Range(1000, 3000, ErrorMessage = "O ano de publicação deve ser um ano válido.")]
        public int? AnoPublicacao { get; set; } // Nullable

        [StringLength(13, MinimumLength = 10, ErrorMessage = "O ISBN deve ter entre 10 e 13 caracteres.")]
        public string? Isbn { get; set; } // Usado para consultar a API externa

        [Range(1, int.MaxValue, ErrorMessage = "O número de páginas deve ser um valor positivo.")]
        public int? NumeroPaginas { get; set; } // Adicionado para o cálculo da sua API

        // Propriedade de navegação para os exemplares deste livro
        public ICollection<Exemplar>? Exemplares { get; set; }

        // Relacionamento Muitos-para-Muitos sem informação da relação com Genero
        public ICollection<LivroGenero>? LivroGeneros { get; set; }
    }
}
