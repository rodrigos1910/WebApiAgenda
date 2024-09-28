using Dapper;

using Microsoft.Extensions.Logging;

using System.Data;

using WebApiAgenda.Interfaces;
using WebApiAgenda.Models;

namespace WebApiAgenda.Repository
{
    public class ContatoRepository : IContatoRepository
    {

        private readonly IDbConnection _dbConnection;
        private readonly ILogger<ContatoRepository> _logger;

        public ContatoRepository(IDbConnection dbConnection, ILogger<ContatoRepository> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }

        public bool Atualizar(long id, Contato contato)
        {
            var linhasAfetadas = _dbConnection.Execute(
                @"UPDATE CONTATO SET Nome = @Nome, Email = @Email, Telefone = @Telefone, Ddd = @Ddd WHERE Id = @Id",
                new { contato.Nome, contato.Email, contato.Telefone, contato.Ddd, Id = id });

            return linhasAfetadas > 0;
        }


        public Contato Criar(Contato contato)
        {
            var comandoSql = @"INSERT INTO CONTATO (Nome, Email, Telefone, Ddd) VALUES (@Nome, @Email, @Telefone, @Ddd);
                               SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = _dbConnection.Query<int>(comandoSql, contato).Single();
            contato.Id = id;
            _logger.LogInformation($"Contato criado com ID {contato.Id}.");
            return contato;
        }
       
        public void Deletar(long id)
        {
            var comandoSql = @"DELETE FROM CONTATO WHERE Id = @Id";
            _logger.LogInformation($"Contato com ID {id} deletado.");
            _dbConnection.Execute(comandoSql, new { Id = id });
        }

       
        public IEnumerable<Contato> Listar(string ddd = null)
        {
            IEnumerable<Contato> contatos;
            if (string.IsNullOrEmpty(ddd))
            {
                var comandoSql = @"SELECT * FROM CONTATO";
                contatos = _dbConnection.Query<Contato>(comandoSql).ToList();
                _logger.LogInformation("Listando todos os contatos.");
            }
            else
            {
                var comandoSql = @"SELECT * FROM CONTATO WHERE Ddd = @Ddd";
                contatos = _dbConnection.Query<Contato>(comandoSql, new { Ddd = ddd }).ToList();
                _logger.LogInformation($"Listando contatos com DDD {ddd}.");
            }

            return contatos;
        }

       
        public Contato Obter(long id)
        {
            var comandoSql = @"SELECT * FROM CONTATO WHERE Id = @ID";
            _logger.LogInformation($"Obtendo contato com ID {id}.");
            return _dbConnection.Query<Contato>(comandoSql, new { ID = id }).SingleOrDefault();
        }
    }
}
