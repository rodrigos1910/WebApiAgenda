using Microsoft.AspNetCore.Mvc;

using WebApiAgenda.Interfaces;
using WebApiAgenda.Models;

namespace WebApiAgenda.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        private readonly ITokenService _tokenService;
        private readonly ILogger<ContatosController> _logger;

        public TokenController(ITokenService tokenService, ILogger<ContatosController> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }


        [HttpPost]

        public IActionResult Post([FromBody] Usuario usuario)
        {

            var token = _tokenService.GetToken(usuario);

            if (!string.IsNullOrEmpty(token))
            {
                return Ok(token);
            }

            return Unauthorized();

        }
    }
}
