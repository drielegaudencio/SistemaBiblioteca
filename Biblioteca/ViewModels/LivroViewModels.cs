// ViewModels/LivroViewModel.cs
using System.ComponentModel.DataAnnotations;
using Biblioteca.Models; // Seus modelos
using Microsoft.AspNetCore.Mvc.Rendering; // Para SelectListItem

namespace Biblioteca.ViewModels
{
    public class LivroViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O autor é obrigatório.")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ISBN é obrigatório.")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "O ISBN deve ter 10 ou 13 caracteres.")]
        public string Isbn { get; set; } = string.Empty;

        [Required(ErrorMessage = "O ano de publicação é obrigatório.")]
        [Range(1000, 9999, ErrorMessage = "O ano de publicação deve ser um valor válido.")]
        public int AnoPublicacao { get; set; }

        [Required(ErrorMessage = "O número de páginas é obrigatório.")]
        [Range(1, 10000, ErrorMessage = "O número de páginas deve ser um valor positivo.")]
        public int NumeroPaginas { get; set; }

        [Required(ErrorMessage = "O gênero principal é obrigatório.")]
        public string GeneroPrincipal { get; set; } = string.Empty; // Isso pode ser mantido para um gênero principal, ou removido se você depender apenas da relação muitos-para-muitos.

        // Propriedade para receber os IDs dos gêneros selecionados no formulário
        public List<int>? SelectedGenreIds { get; set; }

        // Propriedade para popular o dropdown/checkbox list de gêneros na View
        public IEnumerable<SelectListItem>? AvailableGenres { get; set; }

        // Adicione outras propriedades do Livro conforme necessário
    }
}