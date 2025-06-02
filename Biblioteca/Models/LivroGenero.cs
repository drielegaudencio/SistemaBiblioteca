using System.ComponentModel.DataAnnotations.Schema;
using Biblioteca.Models;

// Models / LivroGenero.cs - Entidade de junção para M-M sem informação (apenas chaves)
// Embora o EF Core possa criar uma tabela de junção implícita,
// é boa prática criar a entidade de junção explicitamente para ter controle total,
// mesmo que ela não tenha "informação" além das chaves.
// Isso permite configurar chaves compostas e outras propriedades no DbContext.
namespace Biblioteca.Models
{
    public class LivroGenero
    {
        public int LivroId { get; set; }
        [ForeignKey("LivroId")]
        public  Livro? Livro { get; set; }

        public int GeneroId { get; set; }
        [ForeignKey("GeneroId")]
        public  Genero? Genero { get; set; }
    }
}
