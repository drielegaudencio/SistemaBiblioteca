using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public abstract class Pessoa
    {
        [Key] // Define Id como chave primária
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")] // Campo obrigatório
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(14, ErrorMessage = "O CPF deve ter 14 caracteres (incluindo pontos e traço).")] // Ex: 000.000.000-00
        public required string Cpf { get; set; }

        [StringLength(200, ErrorMessage = "O endereço não pode exceder 200 caracteres.")]
        public string? Endereco { get; set; } // Nullable

        [StringLength(20, ErrorMessage = "O telefone não pode exceder 20 caracteres.")]
        public string? Telefone { get; set; } // Nullable

        [StringLength(100, ErrorMessage = "O email não pode exceder 100 caracteres.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string? Email { get; set; } // Nullable
    }
}
