using LojaProdutosCurso.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace LojaProdutosCurso.Filtros
{
    public class UsuarioLogado : ActionFilterAttribute
    {


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Obtém a string JSON da sessão usando a chave "usuarioSessao"
            string sessao = context.HttpContext.Session.GetString("usuarioSessao");

            if (string.IsNullOrEmpty(sessao))
            {

                // Se a sessão estiver vazia ou nula, redireciona para a página de login
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller", "Login" },
                    {"action", "Login" }
                });
            }
            else
            {
                // Desserializa a string JSON para um objeto UsuarioModel
                UsuarioModel usuarioModel = JsonConvert.DeserializeObject<UsuarioModel>(sessao);

                // Se o usuário não for encontrado, redireciona para a página de login
                if (usuarioModel == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary
                        {
                            {"controller", "Login" },
                            {"action", "Login" }
                        });
                }
            }

            // Chama o método base para garantir que a execução continue normalmente
            base.OnActionExecuting(context);
        }

    }
}