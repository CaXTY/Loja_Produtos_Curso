using LojaProdutosCurso.Models;
using Newtonsoft.Json;

namespace LojaProdutosCurso.Services.Sessao
{
    public class SessaoService : ISessaoInterface
    {
        private readonly IHttpContextAccessor _ctx;

        public SessaoService(IHttpContextAccessor ctx) 
        {
            // Injetando o IHttpContextAccessor para acessar a sessão
            _ctx = ctx;
        }
        public UsuarioModel BuscarSessao()
        {
            // Recuperando o JSON da sessão
            string sessaoUsuario = _ctx.HttpContext.Session.GetString("usuarioSessao");

            // Verificando se a sessão existe
            if (string.IsNullOrEmpty(sessaoUsuario))
            {
                return null;
            }
            // Convertendo o JSON de volta para um objeto UsuarioModel
            return JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);
        }

        public void CriarSessao(UsuarioModel usuario)
        {
            // Convertendo o objeto UsuarioModel para JSON
            string usuarioJson = JsonConvert.SerializeObject(usuario);

            // Armazenando o JSON na sessão
            _ctx.HttpContext.Session.SetString("usuarioSessao", usuarioJson);
        }

        public void RemoverSessao()
        {
            // Removendo a sessão do usuário
            _ctx.HttpContext.Session.Remove("usuarioSessao");
        }
    }
}
