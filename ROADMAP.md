# ROADMAP — SenacGames

> Guia passo a passo para criar a solução SenacGames do zero.
> Voltado para alunos iniciantes em ASP.NET Core MVC e arquitetura em camadas.

---

## Índice

1. [Entendendo a Arquitetura](#1-entendendo-a-arquitetura)
2. [Criação da Solution](#2-criação-da-solution)
3. [Criação das Camadas](#3-criação-das-camadas)
4. [Referências entre Projetos](#4-referências-entre-projetos)
5. [Instalação dos Pacotes NuGet](#5-instalação-dos-pacotes-nuget)
6. [Camada Domain](#6-camada-domain)
7. [Camada Application](#7-camada-application)
8. [Camada Infrastructure](#8-camada-infrastructure)
9. [Entity Framework — Migrations](#9-entity-framework--migrations)
10. [Identity — Autenticação](#10-identity--autenticação)
11. [Projeto API](#11-projeto-api)
12. [Projeto UI (MVC)](#12-projeto-ui-mvc)
13. [Executando a Aplicação](#13-executando-a-aplicação)

---

## 1. Entendendo a Arquitetura

### Por que usar camadas?

A arquitetura em camadas separa o código em projetos com responsabilidades específicas. Isso traz benefícios como:

- **Organização**: cada camada tem uma responsabilidade clara
- **Manutenção**: alterações em uma camada não afetam as outras
- **Testabilidade**: facilita a criação de testes unitários
- **Reutilização**: a mesma lógica pode ser usada por MVC e API

### Estrutura do SenacGames

```
┌─────────────┐   ┌─────────────┐
│ SenacGames │   │ SenacGames │
│   .API  │   │   .UI   │
│ (API REST) │   │  (MVC)  │
└──────┬──────┘   └──────┬──────┘
    │          │
    └────────┬──────────┘
        │
    ┌────────▼────────┐
    │  SenacGames  │
    │ .Application  │
    │ (Serviços/DTOs)│
    └────────┬────────┘
        │
    ┌────────▼────────┐
    │  SenacGames  │
    │  .Domain   │
    │ (Entidades)  │
    └─────────────────┘
        ▲
    ┌────────┴────────┐
    │  SenacGames  │
    │ .Infrastructure │
    │ (EF Core/BD)  │
    └─────────────────┘
```

### Papel de cada camada

| Camada | Tipo de Projeto | Responsabilidade |
|--------|-----------------|-----------------|
| **Domain** | Class Library | Entidades e Interfaces |
| **Application** | Class Library | Services, DTOs, ViewModels |
| **Infrastructure** | Class Library | EF Core, Repositories, Identity |
| **API** | ASP.NET Core Web API | Endpoints REST, Swagger |
| **UI** | ASP.NET Core MVC | Controllers, Views, Bootstrap |

### Fluxo de uma requisição

```
Usuário → Controller → Service → Repository → Banco de Dados
         ↓      ↓      ↓
       ViewModel   DTO    Entidade
```

---

## 2. Criação da Solution

### O que é uma Solution?

Uma **Solution** (.sln) é um arquivo que agrupa vários projetos do .NET. No Visual Studio, é como uma "pasta" que contém todos os seus projetos.

### Via Visual Studio

1. Abra o Visual Studio
2. Clique em **"Criar um novo projeto"**
3. Procure por **"Solução em Branco"**
4. Nome: `SenacGames`
5. Local: escolha uma pasta de sua preferência
6. Clique em **Criar**

### Via terminal (PowerShell ou CMD)

#### Opção 2 — PowerShell

```powershell
# PowerShell:
# Navegue até a pasta onde deseja criar o projeto
cd C:\Users\SeuUsuario\source\repos

# Cria a pasta do projeto
mkdir SenacGames
cd SenacGames

# Cria a Solution
dotnet new sln -n SenacGames
```

#### Opção 3 — Prompt de Comando (CMD)

```cmd
REM CMD:
cd C:\Users\SeuUsuario\source\repos
mkdir SenacGames
cd SenacGames
dotnet new sln -n SenacGames
```

---

## 3. Criação das Camadas

Agora vamos criar cada projeto da solução.

### 3.1 — SenacGames.Domain (Class Library)

**Função**: Contém as entidades e interfaces do sistema. É o "coração" da aplicação.
**Tipo**: Biblioteca de Classes (Class Library)

#### PowerShell

```powershell
# PowerShell:
dotnet new classlib -n SenacGames.Domain -o SenacGames.Domain --framework net8.0
dotnet sln add SenacGames.Domain/SenacGames.Domain.csproj
```

#### CMD

```cmd
REM CMD:
dotnet new classlib -n SenacGames.Domain -o SenacGames.Domain --framework net8.0
dotnet sln add SenacGames.Domain\SenacGames.Domain.csproj
```

### 3.2 — SenacGames.Application (Class Library)

**Função**: Contém a lógica de aplicação — Services, DTOs e ViewModels.

```powershell
# PowerShell:
dotnet new classlib -n SenacGames.Application -o SenacGames.Application --framework net8.0
dotnet sln add SenacGames.Application/SenacGames.Application.csproj
```

### 3.3 — SenacGames.Infrastructure (Class Library)

**Função**: Contém o acesso a dados — Entity Framework Core, Repositories, Identity.

```powershell
# PowerShell:
dotnet new classlib -n SenacGames.Infrastructure -o SenacGames.Infrastructure --framework net8.0
dotnet sln add SenacGames.Infrastructure/SenacGames.Infrastructure.csproj
```

### 3.4 — SenacGames.API (Web API)

**Função**: Expõe endpoints REST com Swagger.

```powershell
# PowerShell:
dotnet new webapi -n SenacGames.API -o SenacGames.API --framework net8.0
dotnet sln add SenacGames.API/SenacGames.API.csproj
```

### 3.5 — SenacGames.UI (MVC)

**Função**: Aplicação web com Controllers, Views Razor e Bootstrap.

```powershell
# PowerShell:
dotnet new mvc -n SenacGames.UI -o SenacGames.UI --framework net8.0
dotnet sln add SenacGames.UI/SenacGames.UI.csproj
```

---

## 4. Referências entre Projetos

### Por que adicionar referências?

Cada camada precisa "enxergar" as camadas abaixo dela. Sem as referências, um projeto não consegue usar as classes de outro projeto.

### Regras de referência

```
Application → Domain
Infrastructure → Domain, Application
API → Application, Infrastructure
UI → Application, Infrastructure
```

#### PowerShell

```powershell
# PowerShell:
# Application depende do Domain
dotnet add SenacGames.Application reference SenacGames.Domain

# Infrastructure depende do Domain e Application
dotnet add SenacGames.Infrastructure reference SenacGames.Domain
dotnet add SenacGames.Infrastructure reference SenacGames.Application

# API depende de Application e Infrastructure
dotnet add SenacGames.API reference SenacGames.Application
dotnet add SenacGames.API reference SenacGames.Infrastructure

# UI depende de Application e Infrastructure
dotnet add SenacGames.UI reference SenacGames.Application
dotnet add SenacGames.UI reference SenacGames.Infrastructure
```

#### Via Visual Studio

1. Clique com o botão direito no projeto → **Adicionar** → **Referência de Projeto**
2. Marque os projetos necessários
3. Clique em **OK**

---

## 5. Instalação dos Pacotes NuGet

### O que são pacotes NuGet?

NuGet é o gerenciador de pacotes do .NET. Os pacotes são bibliotecas prontas que adicionam funcionalidades ao projeto.

### Pacotes necessários

#### SenacGames.Infrastructure

#### Opção 1 — Console do Gerenciador de Pacotes (Package Manager Console — Visual Studio)

Acesse: **Ferramentas → Gerenciador de Pacotes NuGet → Console do Gerenciador de Pacotes**

> ** IMPORTANTE**: No dropdown "Projeto padrão", selecione **SenacGames.Infrastructure**.

```powershell
# Console do Gerenciador de Pacotes:
Install-Package Microsoft.EntityFrameworkCore -Version 8.0.11
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 8.0.11
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.11
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 8.0.11
```

#### Opção 2 — PowerShell

```powershell
# PowerShell:
dotnet add SenacGames.Infrastructure package Microsoft.EntityFrameworkCore --version 8.0.11
dotnet add SenacGames.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.11
dotnet add SenacGames.Infrastructure package Microsoft.EntityFrameworkCore.Tools --version 8.0.11
dotnet add SenacGames.Infrastructure package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.11
```

#### Opção 3 — Prompt de Comando (CMD)

```cmd
REM CMD:
dotnet add SenacGames.Infrastructure package Microsoft.EntityFrameworkCore --version 8.0.11
dotnet add SenacGames.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.11
dotnet add SenacGames.Infrastructure package Microsoft.EntityFrameworkCore.Tools --version 8.0.11
dotnet add SenacGames.Infrastructure package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.11
```

#### SenacGames.API

```powershell
# PowerShell:
dotnet add SenacGames.API package Microsoft.EntityFrameworkCore.Design --version 8.0.11
dotnet add SenacGames.API package Swashbuckle.AspNetCore --version 6.5.0
```

#### SenacGames.UI

```powershell
# PowerShell:
dotnet add SenacGames.UI package Microsoft.EntityFrameworkCore.Design --version 8.0.11
```

### Instalar dotnet-ef (ferramenta global)

```powershell
# PowerShell — Instala a ferramenta globalmente:
dotnet tool install --global dotnet-ef --version 8.0.11
```

> ** Nota**: O `dotnet-ef` é necessário para executar os comandos `dotnet ef migrations` e `dotnet ef database update` via terminal.

---

## 6. Camada Domain

### Estrutura de pastas

```
SenacGames.Domain/
├── Entities/
│  ├── Game.cs
│  └── Category.cs
└── Interfaces/
  ├── IGameRepository.cs
  └── ICategoryRepository.cs
```

### 6.1 — Entidade Game

Crie a pasta `Entities` dentro de `SenacGames.Domain` e adicione o arquivo `Game.cs`:

- **Id**: Chave primária (gerada automaticamente)
- **Title**: Título do game
- **Description**: Descrição do game
- **ReleaseYear**: Ano de lançamento
- **CoverImageUrl**: URL da imagem de capa
- **CategoryId**: Chave estrangeira para Category (relacionamento N:1)
- **IsFeatured**: Se o game está em destaque
- **CreatedAt**: Data de criação
- **Category**: Propriedade de navegação

### 6.2 — Entidade Category

- **Id**: Chave primária
- **Name**: Nome da categoria
- **Games**: Coleção de games (relação 1:N)

### 6.3 — Interfaces de Repositório

As interfaces definem O QUE os repositórios devem fazer, sem definir COMO.

---

## 7. Camada Application

### Estrutura de pastas

```
SenacGames.Application/
├── DTOs/
│  ├── GameDto.cs
│  ├── CategoryDto.cs
│  └── AuthDto.cs
├── Interfaces/
│  ├── IGameService.cs
│  └── ICategoryService.cs
├── Services/
│  ├── GameService.cs
│  └── CategoryService.cs
└── ViewModels/
  └── ViewModels.cs
```

### 7.1 — DTOs (Data Transfer Objects)

DTOs são objetos usados para transferir dados entre camadas. Evitam expor a entidade diretamente.

### 7.2 — Services

Os serviços orquestram as operações:
1. Recebem DTOs do Controller
2. Convertem para Entidades
3. Chamam o Repositório
4. Convertem o resultado para DTO/ViewModel
5. Retornam para o Controller

---

## 8. Camada Infrastructure

### Estrutura de pastas

```
SenacGames.Infrastructure/
├── Context/
│  └── SenacGamesDbContext.cs
├── Configurations/
│  ├── GameConfiguration.cs
│  └── CategoryConfiguration.cs
├── Repositories/
│  ├── GameRepository.cs
│  └── CategoryRepository.cs
├── Identity/
│  └── SeedData.cs
└── Migrations/
  └── (geradas automaticamente)
```

### 8.1 — DbContext

O `SenacGamesDbContext` herda de `IdentityDbContext` para incluir as tabelas do Identity (usuários, roles, etc.).

### 8.2 — Configurações Fluent API

Usamos `IEntityTypeConfiguration<T>` para definir regras do banco de dados:
- Tamanhos máximos de campos
- Campos obrigatórios
- Relacionamentos entre tabelas

### 8.3 — Repositórios

Os repositórios implementam as interfaces do Domain usando Entity Framework Core.

### 8.4 — Seed Data

Dados iniciais que são inseridos na primeira execução:
- 8 categorias (Ação, Aventura, RPG, etc.)
- 8 games de exemplo
- Usuário admin (admin@senacgames.com / Admin@123)

---

## 9. Entity Framework — Migrations

### O que são Migrations?

Migrations são o mecanismo do Entity Framework para criar e atualizar o banco de dados. Cada migration representa uma alteração no esquema do banco.

### Informações importantes

- **Projeto que contém o DbContext**: `SenacGames.Infrastructure`
- **Projeto startup**: `SenacGames.API` (ou `SenacGames.UI`)
- **Banco de dados**: SQL Server LocalDB

### Criar a migration inicial

#### Opção 1 — Console do Gerenciador de Pacotes (Package Manager Console — Visual Studio)

O Package Manager Console usa comandos do Entity Framework **sem** o prefixo `dotnet`.

Acesse: **Ferramentas → Gerenciador de Pacotes NuGet → Console do Gerenciador de Pacotes**

> ** IMPORTANTE**: No dropdown "Projeto padrão", selecione `SenacGames.Infrastructure`.
> Certifique-se de que o projeto de inicialização (startup) é `SenacGames.API`.

```powershell
# Console do Gerenciador de Pacotes:
Add-Migration Inicial -Project SenacGames.Infrastructure -StartupProject SenacGames.API
```

```powershell
# Console do Gerenciador de Pacotes:
Update-Database -Project SenacGames.Infrastructure -StartupProject SenacGames.API
```

---

#### Opção 2 — PowerShell

O PowerShell utiliza a CLI do .NET com comandos `dotnet ef`.

Primeiro, instale o `dotnet-ef` globalmente (se ainda não fez):

```powershell
# PowerShell — Instalar dotnet-ef:
dotnet tool install --global dotnet-ef --version 8.0.11
```

Agora execute os comandos de migration:

```powershell
# PowerShell — Criar a migration:
dotnet ef migrations add Inicial --project SenacGames.Infrastructure --startup-project SenacGames.API
```

```powershell
# PowerShell — Aplicar a migration no banco:
dotnet ef database update --project SenacGames.Infrastructure --startup-project SenacGames.API
```

---

#### Opção 3 — Prompt de Comando (CMD)

O CMD utiliza os mesmos comandos `dotnet ef` do PowerShell.

```cmd
REM CMD — Criar a migration:
dotnet ef migrations add Inicial --project SenacGames.Infrastructure --startup-project SenacGames.API
```

```cmd
REM CMD — Aplicar a migration:
dotnet ef database update --project SenacGames.Infrastructure --startup-project SenacGames.API
```

---

## 10. Identity — Autenticação

### O que é ASP.NET Core Identity?

O Identity é o sistema de autenticação e autorização do ASP.NET Core. Ele gerencia:
- Criação de usuários
- Login e Logout
- Hash de senhas
- Roles (papéis de acesso)
- Cookies de autenticação

### Tabelas criadas pelo Identity

O Identity cria automaticamente as seguintes tabelas no banco:
- `AspNetUsers` — Usuários
- `AspNetRoles` — Papéis (Admin, User, etc.)
- `AspNetUserRoles` — Relacionamento Usuário ↔ Role
- `AspNetUserClaims` — Claims do usuário
- `AspNetRoleClaims` — Claims da role

### Configuração no Program.cs

```csharp
// Registra o Identity com Entity Framework
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
  options.Password.RequireDigit = true;
  options.Password.RequireLowercase = true;
  options.Password.RequireUppercase = true;
  options.Password.RequireNonAlphanumeric = true;
  options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<SenacGamesDbContext>()
.AddDefaultTokenProviders();
```

### Proteção de rotas com [Authorize]

```csharp
// Qualquer usuário autenticado pode acessar
[Authorize]
public class ProfileController : Controller { }

// Apenas admin pode acessar
[Authorize(Roles = "Admin")]
public class AdminController : Controller { }
```

---

## 11. Projeto API

### Estrutura

```
SenacGames.API/
├── Controllers/
│  ├── GamesController.cs
│  ├── CategoriesController.cs
│  └── AuthController.cs
├── Program.cs
└── appsettings.json
```

### Endpoints REST

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| GET | `/api/games` | Lista games | Não |
| GET | `/api/games/{id}` | Busca game | Não |
| POST | `/api/games` | Cria game | Sim (Admin) |
| PUT | `/api/games/{id}` | Atualiza | Sim (Admin) |
| DELETE | `/api/games/{id}` | Remove | Sim (Admin) |
| POST | `/api/auth/login` | Login | Não |
| POST | `/api/auth/register` | Register | Não |
| GET | `/api/auth/me` | Dados usuário | Sim |

### Swagger

Acesse `https://localhost:PORTA/swagger` para ver a documentação automática e testar os endpoints.

---

## 12. Projeto UI (MVC)

### O que é MVC?

- **Model**: Os dados (ViewModels/DTOs)
- **View**: A interface (Razor .cshtml)
- **Controller**: A lógica (recebe requisição → processa → retorna view)

### Estrutura

```
SenacGames.UI/
├── Controllers/
│  ├── HomeController.cs    → Página inicial
│  ├── GamesController.cs   → Catálogo público
│  ├── AccountController.cs  → Login/Register
│  └── AdminController.cs   → Dashboard + CRUD
├── Views/
│  ├── Shared/
│  │  ├── _Layout.cshtml   → Layout público (navbar)
│  │  └── _AdminLayout.cshtml → Layout admin (sidebar)
│  ├── Home/Index.cshtml    → Home page
│  ├── Games/
│  │  ├── Index.cshtml     → Catálogo
│  │  └── Details.cshtml    → Detalhes do game
│  ├── Account/
│  │  ├── Login.cshtml
│  │  ├── Register.cshtml
│  │  └── AccessDenied.cshtml
│  └── Admin/
│    ├── Index.cshtml     → Dashboard
│    ├── Games.cshtml     → Lista de games
│    ├── CreateGame.cshtml  → Cadastrar game
│    ├── EditGame.cshtml   → Editar game
│    ├── DeleteGame.cshtml  → Confirmar exclusão
│    ├── Categories.cshtml  → Lista de categorias
│    ├── CreateCategory.cshtml
│    ├── EditCategory.cshtml
│    └── DeleteCategory.cshtml
└── wwwroot/
  └── css/site.css       → Design customizado
```

### Layouts

- **_Layout.cshtml**: Layout público com navbar azul Senac e footer
- **_AdminLayout.cshtml**: Layout admin com sidebar fixa à esquerda

### Rotas MVC

```
/             → HomeController.Index()
/Games           → GamesController.Index()
/Games/Details/5      → GamesController.Details(5)
/Account/Login       → AccountController.Login()
/Account/Register     → AccountController.Register()
/Admin           → AdminController.Index() (Dashboard)
/Admin/Games        → AdminController.Games()
/Admin/CreateGame     → AdminController.CreateGame()
/Admin/EditGame/5     → AdminController.EditGame(5)
```

---

## 13. Executando a Aplicação

### Compilar a solução

```powershell
# PowerShell:
dotnet build
```

### Executar a API

```powershell
# PowerShell:
dotnet run --project SenacGames.API
```

> O Swagger estará disponível em: `https://localhost:PORTA/swagger`

### Executar a UI (MVC)

```powershell
# PowerShell:
dotnet run --project SenacGames.UI
```

> A aplicação web estará disponível em: `https://localhost:PORTA`

### Login como Admin

1. Acesse `/Account/Login`
2. Email: `admin@senacgames.com`
3. Senha: `Admin@123`
4. Após o login, acesse `/Admin` para o dashboard

---

## Resumo Final

Ao concluir todos os passos deste roadmap, você terá:

- Uma solution com 5 projetos em camadas
- Entidades Game e Category com EF Core
- Repositórios e Services organizados
- API REST com Swagger
- MVC com Views Razor e Bootstrap 5
- Autenticação com Identity (Login, Register, Roles)
- Dashboard administrativo
- CRUD completo de Games e Categorias
- Seed Data com dados iniciais
- Design moderno baseado no protótipo Stitch

**Parabéns!** Você construiu uma aplicação completa e profissional do zero!
