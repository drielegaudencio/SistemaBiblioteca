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
            /*if (context.Planos.Any())
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
*/
            if (context.Exemplares.Any())
            {
                return;
            }
            var exemplares = new Exemplar[]
            {
                new Exemplar {
                   Id=1 ,LivroId= 1, CodigoInventario="5641",Disponivel=true
                }

            };
            foreach (Exemplar exemplar in exemplares)
            {
                context.Exemplares.Add(exemplar);
                context.SaveChanges();

            }

        }

    }

}

