using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using WebApiAgenda.Interfaces;
using WebApiAgenda.Models;

namespace WebApiAgenda.Service
{
    public class TokenService : ITokenService
    {


        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly IUsuarioRepository _usuarioRepository;

        private const int EXPIRES_IN = 24;

        public TokenService(IConfiguration configuration, IMemoryCache cache, IUsuarioRepository usuarioRepository)
        {
            _configuration = configuration;
            _cache = cache;
            _usuarioRepository = usuarioRepository;
        }

        public string GetToken(Usuario usuario)
        {

            // Verifica se o usuário já possui um token no cache
            if (_cache.TryGetValue(usuario.UserName, out string tokenExistente))
            {
                return tokenExistente;
            }

            //valida se o usuario existe no banco de dados
            var usuarioEncontrado = _usuarioRepository.Listar().Any(user => user.UserName.Equals(usuario.UserName) && user.Password.Equals(usuario.Password));

            if (!usuarioEncontrado)
            {
                return string.Empty;
            }

            // variavel para gerar do token
            var tokenHandler = new JwtSecurityTokenHandler();

            //recupera a chave
            var chaveCriptografada = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretJWT"));


            //define todas as propriedades

            var tokenPropriedade = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,usuario.UserName),
                    new Claim(ClaimTypes.Role, (usuario.PermisaoSistema - 1).ToString())

                }),

                //tempo de expiracao
                Expires = DateTime.UtcNow.AddHours(EXPIRES_IN),

                //adiciona a chave criptografada

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(chaveCriptografada), SecurityAlgorithms.HmacSha256Signature)


            };


            // Cria e retorna o token
            var token = tokenHandler.CreateToken(tokenPropriedade);
            var tokenString = tokenHandler.WriteToken(token);

            // Armazena o token no cache
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(EXPIRES_IN) // mesmo tempo de expiração do token
            };

            _cache.Set(usuario.UserName, tokenString, cacheOptions);

            return tokenString;



        }
    }
}
