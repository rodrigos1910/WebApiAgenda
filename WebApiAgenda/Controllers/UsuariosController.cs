using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebApiAgenda.Enums;
using WebApiAgenda.Interfaces;
using WebApiAgenda.Logging;
using WebApiAgenda.Models;
using WebApiAgenda.Repository;

namespace WebApiAgenda.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<ContatosController> _logger;


        public UsuariosController(IUsuarioRepository usuarioRepository, ILogger<ContatosController> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        /// <summary>
        /// Metodo responsavel por listar todos os usuarios
        /// </summary>
        /// <returns>Retorna uma lista de usuarios</returns>
        [HttpGet]
        [Authorize ]
        public IActionResult GetUsuarios()
        {
            return Ok(_usuarioRepository.Listar());
        }
        /// <summary>
        /// Metodo responsavel por retornar um usuario
        /// por meio do seu id
        /// </summary>
        /// <param name="id">id do usuario cadastrado na base</param>
        /// <returns>Retorna um objeto usuario se encontrado</returns>
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetUsuario(long id)
        {

            var usuario = _usuarioRepository.Obter(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }
        /// <summary>
        /// Metodo responsavel por criar os dados de um Usuario
        /// </summary>
        /// <param name="usuario">Objeto usuario</param>
        /// <returns>retorna o objeto usuario que inseriu na base</returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Criar([FromBody] Usuario usuario)
        {
            return Created("",_usuarioRepository.Criar(usuario));
        }


        /// <summary>
        /// Metodo responsavel por atualizado os dados de um Usuario
        /// com base no seu id de cadastro
        /// </summary>
        /// <param name="usuario">Objeto usuario, cadastrado na base</param>
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Atualizar(long id, [FromBody] Usuario usuario)
        {
            if (usuario == null || id != usuario.Id)
            {
                return BadRequest();
            }

            var usuarioExistente = _usuarioRepository.Obter(id);
            if (usuarioExistente == null)
            {
                return NotFound();
            }

            _usuarioRepository.Atualizar(usuario);
            return NoContent();
        }

        /// <summary>
        /// Metodo responsavel por inativar um usuario
        /// com base no seu id
        /// </summary>
        /// <param name="id">Id do usuario cadastrado na base</param>
        [HttpDelete("{id}")]

        [Authorize(Roles = PermissaoSistema.Administrador)]
        public IActionResult Inativar(long id)
        {
            var usuario = _usuarioRepository.Obter(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _usuarioRepository.Inativar(id);
            return NoContent();
        }
    }
}
