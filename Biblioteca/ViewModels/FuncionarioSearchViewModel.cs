// ViewModels/FuncionarioSearchViewModel.cs
using System.Collections.Generic;
using Biblioteca.Models; // Ajuste este namespace se o seu modelo estiver em outro lugar

namespace Biblioteca.ViewModels
{
    public class FuncionarioSearchResultViewModel
    {
        // Informações Pessoais do Funcionário
        public FuncionarioViewModel? Funcionario { get; set; }

        // Lista de Assinaturas Realizadas (Nome da propriedade corrigido)
        public List<AssinaturaViewModel>? AssinaturasRealizadas { get; set; }

        public List<EmprestimoViewModel>? EmprestimosRealizados { get;  set; }
    }

    public class FuncionarioViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Matricula { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty; // Adicionado para mais detalhes do funcionário
        public string Cpf { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public decimal Salario { get; set; }
    }

    public class AssinaturaViewModel
    {
        public int Id { get; set; }
        // Propriedades para exibir o nome do usuário e o tipo de plano
        public string NomeUsuario { get; set; } = string.Empty;
        public string Plano { get; set; } = string.Empty;

        public string DataInicio { get; set; } = string.Empty;
        public string DataFim { get; set; } = string.Empty;
        public bool Ativa { get; set; }
        
    }

    
     public class EmprestimoViewModel
     {
         public int Id { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public string TituloLivro { get; set; } = string.Empty;
         public string IsbnLivro { get; set; } = string.Empty;
         public string DataEmprestimo { get; set; } = string.Empty;
         public string DataDevolucaoPrevista { get; set; } = string.Empty;
         public string DataDevolucaoReal { get; set; } = string.Empty;
         public bool Devolvido { get; set; }
     }
}
