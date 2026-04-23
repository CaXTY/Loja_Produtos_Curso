using AspNetCoreGeneratedDocument;
using LojaProdutosCurso.Dto.Login;
using LojaProdutosCurso.Services.Sessao;
using LojaProdutosCurso.Services.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace LojaProdutosCurso.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioInterface _usuarioInterface;
        private readonly ISessaoInterface _sessaoInterface;

        public LoginController(IUsuarioInterface usuarioInterface, ISessaoInterface sessaoInterface) 
        {
            _usuarioInterface = usuarioInterface;
            _sessaoInterface = sessaoInterface;
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Sair()
        {
            _sessaoInterface.RemoverSessao();
            return RedirectToAction("Login", "Login");

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUsuarioDto loginUsuarioDto)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _usuarioInterface.Login(loginUsuarioDto);

                if (usuario == null)
                {
                    TempData["MensagemErro"] = "Crendecias Inválidas!";
                    return RedirectToAction("Index", "Home");
                }

                TempData["MensagemSucesso"] = "Usuário logado com sucesso.";
                return RedirectToAction("Index", "Home");

            }
            else
            {
                TempData["MensagemErro"] = "Dados de login inválidos. Por favor, verifique suas credenciais.";
                return View(loginUsuarioDto);
            }
        }
    }
}
