// =============================================================================
// SenacGames.UI - AccountController
// =============================================================================
// 📌 CONCEITO: Autenticação MVC
// Este controller gerencia Login, Logout e Registro de usuários.
// Utiliza o ASP.NET Core Identity para autenticação com cookies.
//
// FLUXO DE LOGIN:
// 1. Usuário acessa /Account/Login (GET)
// 2. Preenche email e senha no formulário
// 3. Envia o formulário (POST)
// 4. SignInManager verifica as credenciais
// 5. Se correto: cria cookie de autenticação e redireciona
// 6. Se errado: exibe mensagem de erro
// =============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SenacGames.Application.DTOs;


namespace SenacGames.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //=============================================
        // LOGIN
        //=============================================

        //GET /Account/Login

        //Exibe o formulário do Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // Processa o login do usuário
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            //Tenta fazer login
            var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                //Redireciona para a URL anterior ou para a Home
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            // Se falhou, exibe a mensagem de erro
            ModelState.AddModelError(string.Empty, "Email ou senha inválidos.");
            return View(dto);
        }


        //=============================================
        // REGISTER
        //=============================================

        //Exibe o formulário de registro
        // GET /Account/Register

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //Processa o registro de um novo usuário
        //POST /Account/Register

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "As senhas não coincidem");
                return View(dto);
            }

            var user = new IdentityUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                //Faz Login automático após o registro
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            // Se falhou, exibe os erros
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(dto);
        }

        //=============================================
        // LOGOUT
        //=============================================

        //Faz Login do usuário.
        //POST /Account/Logout

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //=============================================
        // ACCESS DENIED
        //=============================================

        //Página do acesso negado

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}






//Responsabilidade das Views: Receber o modelo de dados processado pelo
//controller e exibir a interface para o usuário. As Views são arquivos .cshtml


//O que é o Razor e para que ele existe? 
//Toda aplicação web precisa devolver HTML para o navegador.
//O Razor é uma sintaxe que permite misturar código C# com HTML de forma simples e intuitiva.


//O símbolo @ é a chave para usar o Razor. Ele indica que o que vem a seguir é código C#.
//Exemplo: @game.Title → Exibe o título do jogo


// A pasta Shared e os layouts: A pasta Shared contém Views que são usadas em toda
// a aplicação, como o layout principal (Layout.cshtml) que define a estrutura
// comum das páginas (cabeçalho, rodapé, etc). As outras Views "herdam" esse
// layout para manter uma aparência consistente.

//ViewStart e ViewImports: O arquivo _ViewStart.cshtml é executado antes de cada View
//e define o layout padrão. O _ViewImports.cshtml é usado para importar namespaces
//e definir diretivas comuns para todas as Views, evitando repetição de código.


//Ordem do que acontece em cada request:
//1. Controller executa a lógica e monta um ViewModel
//2. _ViewStart é executado, definindo o layout 
//3. _ViewImports é executado, importando namespaces e carregando @using e Tag helpers  
//4. A View específica é processada
//5. Entra no @RenderBody() do layout, onde o conteúdo da View é inserido
//6. O HTML final é enviado para o navegador do usuário
//7. O navegador renderiza a página para o usuário ver e interagir.


// Tag Helpers: São componentes que facilitam a criação de elementos
// HTML complexos, como formulários, links, etc. Exemplo:
// <form asp-action="Login"> cria um formulário que envia para a ação Login do controller atual.
