using LojaProdutosCurso.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LojaProdutosCurso.ViewComponents
{
    public class Menu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Recupera a sessão do usuário
            string sessaoUsuario = HttpContext.Session.GetString("usuarioSessao");

            // Verifica se a sessão do usuário existe
            if (string.IsNullOrEmpty(sessaoUsuario)) 
            return View();

            // Desserializa a sessão do usuário para obter os dados do usuário
            UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);

            return View(usuario);
        }
    }
}
