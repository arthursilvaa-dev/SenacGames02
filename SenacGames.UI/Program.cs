// =============================================================================
// SenacGames.UI - Program.cs
// =============================================================================
// 📌 CONCEITO: Este é o ponto de entrada da aplicação MVC (Web).
// Aqui configuramos o servidor web que serve as páginas HTML (Razor Views).
//
// A diferença para o Program.cs da API:
// - API: retorna JSON (dados) — AddControllers()
// - MVC: retorna HTML (páginas) — AddControllersWithViews()
// =============================================================================

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SenacGames.Application.Interfaces;
using SenacGames.Application.Services;
using SenacGames.Domain.Interfaces;
using SenacGames.Infrastructure.Context;
using SenacGames.Infrastructure.Identity;
using SenacGames.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ======================================
// ENTITY FRAMEWORK CORE - Banco de Dados
// ======================================
// Conceito: Configura o EF Core para usar o SQL Server
builder.Services.AddDbContext<SenacGamesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =============================================
// ASP.NET IDENTITY - Autenticação e Autorização
// =============================================
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    //Options: Configura as regras de senha (exemplo: exigir letra maiúscula, número, etc.)
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
})
// Configura o Identity para usar o EF Core e a nossa DbContext personalizada (SenacGamesDbContext)
.AddEntityFrameworkStores<SenacGamesDbContext>()
.AddDefaultTokenProviders();

//Configuração dos cookies de autenticação 
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Redireciona para Página de login
    options.LogoutPath = "/Account/Logout"; // Redireciona para Página de logout
    options.AccessDeniedPath = "/Account/AccessDenied"; // Redireciona para Página de acesso negado
});

// ========================================================================
// DEPENDENCY INJECTION - Injeção de Dependências | Repositórios e Serviços
// ========================================================================
// CONCEITO: Registramos as dependências para que possam ser injetadas
// nos controladores e outros serviços.

// AddScoped: Cria uma nova instância do serviço para cada requisição HTTP.
// Ideal para serviços que precisam de um ciclo de vida curto, como repositórios e serviços de aplicação.
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// ========================================================================
// MVC - Adiciona suporte para controladores e views (páginas HTML) | Razor
// ========================================================================
//AddControllersWithViews: Configura o ASP.NET Core para usar o padrão MVC,
//permitindo retornar páginas HTML renderizadas (Razor Views) a partir dos controladores.
builder.Services.AddControllersWithViews();

//Cria a aplicação a partir do Builder configurado
var app = builder.Build();

// =====================================================================================
// PIPELINES DE MIDDLEWARE - Configura a sequência de processamento das requisições HTTP
// =====================================================================================
// Conceito: Middlewares são componentes que processam as requisições HTTP em uma sequência definida.
if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Redireciona para a página de erro em produção
    app.UseHsts(); // Força o uso de HTTPS em produção
}

// Redireciona HTTP para HTTPS e serve arquivos estáticos (CSS, JS, Imagens) da pasta wwwroot
app.UseHttpsRedirection();
app.UseStaticFiles(); // Permite servir arquivos estáticos (CSS, JS, Imagens) da pasta wwwroot

// Configura o roteamento das requisições para os controladores(controllers) e ações.
app.UseRouting();

// Configura a autenticação e autorização para proteger rotas que exigem
// login ou permissões específicas.

//IMPORTANTE: A ordem dos middlewares é importante! UseAuthentication
//deve vir antes de UseAuthorization, pois a autenticação precisa ocorrer
//antes da autorização para que as permissões sejam verificadas corretamente.

app.UseAuthentication(); // Habilita a autenticação (login)
app.UseAuthorization(); // Habilita a autorização (permissões)

// =================================
// ROTAS - Configuração de rotas MVC
// =================================

//CONCEITO: Rota padrão para os controladores MVC. O formato é:
// /{controller=Home}/{action=Index}/{id?}
//Significa: /NomeController/NomeAcao/IdOpcional
//Exemplo: /Games/Details/5 -> GamesController, Details action, id = 5
app.MapControllerRoute(
    //name: Nome da rota (pode ser usado para referenciar a rota em outros lugares, como links)
    //pattern: Define o formato da URL para as rotas MVC. O formato é: /{controller=Home}/{action=Index}/{id?}
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

//Seed Data: Popula o banco de dados com dados iniciais (categorias e jogos) se estiver vazio.
await SeedData.SeedAsync(app.Services);

// Inicia o servidor web e começa a ouvir as requisições HTTP.
app.Run();

