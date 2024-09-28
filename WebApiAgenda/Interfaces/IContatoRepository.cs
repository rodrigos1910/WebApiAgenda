using WebApiAgenda.Models;

namespace WebApiAgenda.Interfaces
{
    public interface IContatoRepository
    {

        // contratos
        public IEnumerable<Contato> Listar(string ddd = null);
        public Contato Obter(long id);
        public  Contato Criar(Contato contato);
        public bool Atualizar(long id, Contato contato);
        public void Deletar(long id);

    }
}
