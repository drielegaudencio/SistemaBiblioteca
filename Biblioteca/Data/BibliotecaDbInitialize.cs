using Biblioteca.Models;

namespace Biblioteca.Data
{
    public class BibliotecaDbInitialize
    {
        public static void Initialize(BibliotecaDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Funcionarios.Any())
            {
                return;
            }
            var funcionarios = new Funcionario[]
            {
                new Funcionario {
                    Nome = "Mario", Cpf = "654984654646", Endereco = "Rua A", Telefone = "24855555", Email = "mario@mail.com", Cargo = "Atendente", Salario = 1300, Matricula = "55555"
                }

            };
            foreach (Funcionario funcionario in funcionarios)
            {
                context.Funcionarios.Add(funcionario);
                context.SaveChanges();

            }
            if (context.Planos.Any())
            {
                return ;
            }
            var planos = new Plano[] { 
                new Plano
                {
                    Nome = "Plano A", Descricao="Plano simples, apenas acesso ao espaço", LimiteEmprestimosMensais=0, PrazoDevolucaoDias=0, TaxaAtrasoDiariaPercentual=0, ValorMensal=59.99M, Id= 1
                },
                new Plano
                {
                    Nome = "Plano B",
                    Descricao="Plano medio, permite empréstimo de 1 livro",
                    LimiteEmprestimosMensais=1,
                    PrazoDevolucaoDias=30,
                    TaxaAtrasoDiariaPercentual=0.5M,
                    ValorMensal=79,
                    Id= 2
                }
            };
            
            if (context.Assinaturas.Any())
            {
                return ;
            }
            var assinaturas = new Assinatura[]
            {
                new Assinatura
                {
                    Ativa=true, DataInicio= DateTime.Today, PlanoId= 1,UsuarioId= 1, FuncionarioResponsavelId = 1,Id= 2
                }
            };

        }

    }

}

