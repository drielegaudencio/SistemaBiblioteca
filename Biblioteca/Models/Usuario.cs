using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Usuario : Pessoa
    {
        // Chave estrangeira para a assinatura ativa do usuário (pode ser nula se não tiver assinatura)
        public int? AssinaturaAtualId { get; set; }
        // Propriedade de navegação para a assinatura ativa
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.
        public Assinatura AssinaturaAtual { get; set; }
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.

        // Histórico de todas as assinaturas que o usuário já teve
        public ICollection<Assinatura>? HistoricoAssinaturas { get; set; }

        // Empréstimos que o usuário realizou
        public ICollection<Emprestimo>? Emprestimos { get; set; }

        // Propriedade de conveniência para verificar se o usuário é assinante ativo
        [NotMapped] // Não mapeia esta propriedade para o banco de dados
        public bool IsAssinanteAtivo => AssinaturaAtual != null && AssinaturaAtual.Ativa && AssinaturaAtual.DataFim >= DateTime.Today;
    }
}
