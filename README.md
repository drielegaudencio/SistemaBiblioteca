# SistemaBiblioteca
Visão Geral do Projeto
Este é um projeto ASP.NET Core que combina funcionalidades de MVC (para as interfaces de usuário) e Web API (para sua API personalizada).

## Tecnologias Utilizadas:

ASP.NET Core 8.0: Framework para construir a aplicação web e API.\
Entity Framework Core 8.0: ORM para persistência de dados em um banco de dados SQL Server (LocalDB para desenvolvimento).\
Swagger/Swashbuckle: Para documentar e testar sua API personalizada.\
HttpClient: Para consumir a API externa do Open Library.\
Razor Views e Partial Views: Para a interface do usuário.\

## Estrutura de Pastas :

Biblioteca/\
├── Models/             // Classes de entidade (Pessoa, Livro, Assinatura, etc.) <br/>
├── Data/               // DbContext e configurações do EF Core\
├── Services/           // Lógica de negócio e integração com APIs externas\
├── Controllers/        // Controladores MVC e API\
├── Views/              // Views Razor (.cshtml)\
│   └── Assinaturas/    // Views específicas para assinaturas\
│   └── Emprestimos/    // Views específicas para emprestimos\
│   └── Funcionarios/   // Views específicas para funcionarios\
│   └── Home/           // Views específicas para home\
│   ├── Livros/         // Views específicas para livros\
│   └── Planos/         // Views específicas para planos\
│   ├── Shared/         // Partial Views\
│   ├── Usuarios/       // Views específicas para usuarios\
├── Migrations/         // Geradas pelo EF Core\
├── appsettings.json    // Configurações do banco de dados\
├── Program.cs          // Ponto de entrada da aplicação e configuração de serviços
