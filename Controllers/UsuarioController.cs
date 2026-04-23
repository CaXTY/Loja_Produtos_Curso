using AutoMapper;
using LojaProdutosCurso.Dto.Endereco;
using LojaProdutosCurso.Dto.Usuario;
using LojaProdutosCurso.Filtros;
using LojaProdutosCurso.Services.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace LojaProdutosCurso.Controllers
{
    [UsuarioLogado]
    [UsuarioLogadoAdm]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioInterface _usuarioInterface;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioInterface usuarioInterface, IMapper mapper)
        {
            _usuarioInterface = usuarioInterface;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioInterface.BuscarUsuario();

            return View(usuarios);
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        public async Task<IActionResult> Editar(int id)
        {
            var usuario = await _usuarioInterface.BuscarUsuarioPorId(id);

            var usuarioEditado = new EditarUsuarioDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Cargo = usuario.Cargo,
                Endereco = _mapper.Map<EditarEnderecoDto>(usuario.Endereco)
            };

            return View(usuarioEditado);

        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(CriarUsuarioDto criarUsuarioDto)
        {
            if (ModelState.IsValid)
            {
                if (await _usuarioInterface.VerificaSeExisteEmail(criarUsuarioDto))
                {
                    TempData["MensagemErro"] = "Já existe um usuário cadastrado com este email.";
                    return View(criarUsuarioDto);
                }

                var usuario = await _usuarioInterface.Cadastrar(criarUsuarioDto);

                TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso.";

                return RedirectToAction("Index");
            }
            else
            {
                TempData["MensagemErro"] = "Por favor, preencha todos os campos corretamente.";
                return View(criarUsuarioDto);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Editar(EditarUsuarioDto editarUsuarioDto)
        {
            if (ModelState.IsValid)
            {
                //Aqui você pode implementar a lógica para atualizar o usuário no banco de dados
                //Utilizando o serviço de usuário para realizar a atualização

                var usuario = await _usuarioInterface.Editar(editarUsuarioDto);
                TempData["MensagemSucesso"] = "Usuário editado com sucesso.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["MensagemErro"] = "Por favor, preencha todos os campos corretamente.";

                return View(editarUsuarioDto);
            }


        }
    }
}
