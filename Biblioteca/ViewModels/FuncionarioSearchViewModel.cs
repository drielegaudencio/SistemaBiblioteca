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

        // Removido EmpréstimosViewModel conforme a nova requisição de focar apenas em assinaturas
        // public List<EmprestimoViewModel>? EmprestimosRealizados { set; }
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
        // Não é necessário FuncionarioResponsavelId, UsuarioId, PlanoId aqui para exibição simples
        // public int FuncionarioResponsavelId { get; set; }
        // public int UsuarioId { get; set; }
        // public int PlanoId { get; set; }

        // Propriedades para exibir o nome do usuário e o tipo de plano
        public string NomeUsuario { get; set; } = string.Empty;
        public string TipoPlano { get; set; } = string.Empty;

        public string DataInicio { get; set; } = string.Empty;
        public string DataFim { get; set; } = string.Empty;
        public bool Ativa { get; set; }
        // ValorCobrado REMOVIDO daqui pois está comentado no seu modelo Assinatura.cs
        // public decimal ValorCobrado { get; set; }
    }

    // EmprestimoViewModel não é mais necessário para este cenário de busca de funcionário focado em assinaturas
    // public class EmprestimoViewModel
    // {
    //     public int Id { get; set; }
    //     public string TituloLivro { get; set; } = string.Empty;
    //     public string IsbnLivro { get; set; } = string.Empty;
    //     public string DataEmprestimo { get; set; } = string.Empty;
    //     public string DataDevolucaoPrevista { get; set; } = string.Empty;
    //     public string DataDevolucaoReal { get; set; } = string.Empty;
    //     public bool Devolvido { get; set; }
    // }
}
