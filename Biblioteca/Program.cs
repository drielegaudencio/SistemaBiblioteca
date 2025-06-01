// Program.cs
using Microsoft.EntityFrameworkCore;
using Biblioteca.Data; // Ajustado para o namespace raiz 'Biblioteca'
using Biblioteca.Services; // Ajustado para o namespace raiz 'Biblioteca'
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner.

// 1. Configuração do DbContext para Entity Framework Core
// Certifique-se de que o namespace da sua classe DbContext está correto aqui.
builder.Services.AddDbContext<BibliotecaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BibliotecaDbConnection")));

// 2. Adiciona controladores com views (para MVC)
builder.Services.AddControllersWithViews();

// 3. Adiciona HttpClient para o serviço OpenLibraryService
// É crucial que esta linha esteja presente e que o namespace esteja correto.
builder.Services.AddHttpClient<OpenLibraryService>();

// 4. Configuração do Swagger/OpenAPI para documentar a API personalizada
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API da Biblioteca - Cálculo de Leitura",
        Version = "v1",
        Description = "API para calcular o tempo estimado de leitura de um livro com base em hábitos de leitura.",
        Contact = new OpenApiContact
        {
            Name = "Seu Nome/Empresa",
            Email = "seu.email@example.com",
            Url = new Uri("https://seusite.com")
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
    else
    {
        Console.WriteLine($"Arquivo XML de documentação não encontrado em: {xmlPath}");
        Console.WriteLine("Certifique-se de que 'GenerateDocumentationFile' está definido como 'true' no .csproj e que o projeto foi compilado.");
    }
});


var app = builder.Build();

// Configura o pipeline de requisições HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API da Biblioteca v1");
        c.RoutePrefix = "swagger";
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<BibliotecaDbContext>();
        BibliotecaDbInitialize.Initialize(context);

    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao popular o banco de Dados");
    }
}
app.MapControllers();

app.Run();



/*using Biblioteca.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BibliotecaDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BibliotecaDbConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<BibliotecaDbContext>();
        BibliotecaDbInitialize.Initialize(context);

    }
    catch(Exception ex)
    {
        var logger =services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao popular o banco de Dados");
    }
}

app.Run();
*/