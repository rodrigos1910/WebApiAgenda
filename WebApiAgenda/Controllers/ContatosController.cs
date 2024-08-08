using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebApiAgenda.Enums;
using WebApiAgenda.Interfaces;
using WebApiAgenda.Logging;
using WebApiAgenda.Models;

namespace WebApiAgenda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatosController : ControllerBase
    {

        private readonly IContatoRepository _contatoRepository;
        private readonly ILogger<ContatosController> _logger;

        public ContatosController(IContatoRepository contatoRepository, ILogger<ContatosController> logger)
        {
            _contatoRepository = contatoRepository;
            _logger = logger;
        }
        /// <summary>
        /// Lista todos os contatos do banco de dados.
        /// </summary>
        /// <param name="ddd">O DDD opcional para filtrar contatos.</param>
        /// <remarks>
        /// Exemplo de resposta:
        /// 
        ///     GET /api/contatos
        ///     [
        ///         {
        ///             "id": 1,
        ///             "nome": "Nome do Contato",
        ///             "ddd": "11",
        ///             "telefone": "999999999",
        ///             "email": "email@exemplo.com"
        ///         }
        ///     ]
        /// 
        /// Exemplo de resposta com filtro por DDD:
        /// 
        ///     GET /api/contatos?ddd=11
        ///     [
        ///         {
        ///             "id": 1,
        ///             "nome": "Nome do Contato",
        ///             "ddd": "11",
        ///             "telefone": "999999999",
        ///             "email": "email@exemplo.com"
        ///         }
        ///     ]
        /// </remarks>
        /// <returns>Uma lista de todos os contatos.</returns>
        /// <response code="200">Retorna a lista de contatos</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetContatos([FromQuery] string ddd = null)
        {
            try
            {
                var contatos = _contatoRepository.Listar(ddd);
                return Ok(contatos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar contatos.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao listar contatos.");
            }
        }
        /// <summary>
        /// Obtém um contato específico pelo ID.
        /// </summary>
        /// <param name="id">O ID do contato a ser obtido.</param>
        /// <remarks>
        /// Exemplo de resposta:
        /// 
        ///     GET /api/contatos/1
        ///     {
        ///         "id": 1,
        ///         "nome": "Nome do Contato",
        ///         "ddd": "11",
        ///         "telefone": "999999999",
        ///         "email": "email@exemplo.com"
        ///     }
        /// </remarks>
        /// <returns>O contato correspondente ao ID.</returns>
        /// <response code="200">Retorna o contato encontrado</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="404">Contato não encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetContato(long id)
        {
            try
            {
                var contato = _contatoRepository.Obter(id);
                if (contato == null)
                {
                    return NotFound("Contato não encontrado.");
                }
                _logger.LogInformation("Contato obtido com sucesso.");
                return Ok(contato);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contato.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter contato.");
            }
        }
        /// <summary>
        /// Cria um novo contato no banco de dados.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/contatos
        ///     {
        ///         "nome": "Nome do Contato",
        ///         "ddd": "11",
        ///         "telefone": "999999999",
        ///         "email": "email@exemplo.com"
        ///     }
        /// 
        /// Obs.: Não é necessário informar o ID, ele será gerado automaticamente.
        /// </remarks>
        /// <param name="contato">O contato a ser criado.</param>
        /// <returns>O contato criado com o ID gerado.</returns>
        /// <response code="201">Contato criado com sucesso</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Criar(Contato contato)
        {
            if (contato == null)
            {
                return BadRequest("Dados inválidos.");
            }

            try
            {
                var contatoCriado = _contatoRepository.Criar(contato);
                return CreatedAtAction(nameof(GetContato), new { id = contatoCriado.Id }, contatoCriado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar contato.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar contato.");
            }
        }
        /// <summary>
        /// Atualiza um contato existente no banco de dados.
        /// </summary>
        /// <param name="contato">O contato com as informações atualizadas.</param>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     PUT /api/contatos/1
        ///     {
        ///         "id": 1,
        ///         "nome": "Nome do Contato",
        ///         "ddd": "11",
        ///         "telefone": "999999999",
        ///         "email": "email@exemplo.com"
        ///     }
        /// 
        /// Obs.: O ID deve ser informado e corresponder ao contato a ser atualizado.
        /// </remarks>
        /// <response code="200">Contato atualizado com sucesso</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="400">Dados inválidos</response>
        /// <response code="404">Contato não encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Atualizar(long id, [FromBody] Contato contato)
        {
            try
            {
                var contatoExistente = _contatoRepository.Obter(id);
                if (contatoExistente == null)
                {
                    return NotFound("Contato não encontrado.");
                }

                _contatoRepository.Atualizar(id, contato);
                return Ok("Contato atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar contato.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar contato.");
            }
        }

        /// <summary>
        /// Deleta um contato do banco de dados.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     DELETE /api/contatos/1
        /// 
        /// Obs.: O ID do contato deve ser informado.
        /// </remarks>
        /// <param name="id">O ID do contato a ser deletado.</param>
        /// <response code="200">Contato deletado com sucesso</response>
        /// <response code="401">Não autorizado</response>
        /// <response code="404">Contato não encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpDelete("{id}")]
        [Authorize ]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult Remover(long id)
        {
            try
            {
                var contatoExistente = _contatoRepository.Obter(id);
                if (contatoExistente == null)
                {
                    return NotFound("Contato não encontrado.");
                }

                _contatoRepository.Deletar(id);
                return Ok("Contato deletado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar contato.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar contato.");
            }
        }



       
    }
}
