using WebApiAgenda.Models;

namespace WebApiAgenda.Interfaces
{
    public interface IUsuarioRepository
    {
       
        // contratos
        public IEnumerable<Usuario> Listar();
        public Usuario Obter(long id);
        public Usuario Criar(Usuario usuario);
        public void Atualizar(Usuario usuario);
        public void Inativar(long id);
    }
}
