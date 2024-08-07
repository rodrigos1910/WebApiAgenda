using WebApiAgenda.Models;

namespace WebApiAgenda.Interfaces
{
    public interface ITokenService
    {
        public string GetToken(Usuario usuario);
    }
}
