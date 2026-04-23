using LojaProdutosCurso.Enums;
using LojaProdutosCurso.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace LojaProdutosCurso.Filtros
{
    public class UsuarioLogadoAdm : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Verificar se o usuário está logado
            var sessao = context.HttpContext.Session.GetString("usuarioSessao");


            if (string.IsNullOrEmpty(sessao))
            {
                // Redirecionar para a página de login
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller", "Login" },
                    {"action", "Login" }
                });
            }
            else
            {
                // Desserializar o objeto do usuário a partir da sessão
                UsuarioModel usuarioModel = JsonConvert.DeserializeObject<UsuarioModel>(sessao);

                
                if (usuarioModel.Cargo == CargoEnum.Cliente)
                {
                    // Redirecionar para a página inicial
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary
                        {
                            {"controller", "Home" },
                            {"action", "Index" }
                        });
                }
            }
            // Chamar o método base para continuar a execução da ação
            base.OnActionExecuting(context);
        }


    }
}