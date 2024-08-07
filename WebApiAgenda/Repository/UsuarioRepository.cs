using Dapper;

using System.Data;

using WebApiAgenda.Interfaces;
using WebApiAgenda.Models;

namespace WebApiAgenda.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly IDbConnection _dbConnection;
        private readonly ILogger<UsuarioRepository> _logger;


        public UsuarioRepository(IDbConnection dbConnection, ILogger<UsuarioRepository> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }


        public void Atualizar(Usuario usuario)
        {
           /* var usuarioExistente = ListaUsuario.Usuarios.FirstOrDefault(u => u.Id == usuario.Id);
            if (usuarioExistente != null)
            {
                usuarioExistente.UserName = usuario.UserName;
                usuarioExistente.Password = usuario.Password;
                usuarioExistente.Ativo = usuario.Ativo;
                usuarioExistente.PermisaoSistema = usuario.PermisaoSistema;
            }*/

            var comandoSql = @"UPDATE USUARIO SET UserName = @UserName, Password = @Password, Ativo = @Ativo, PermisaoSistema = @PermisaoSistema WHERE Id = @Id";
            _dbConnection.Execute(comandoSql, usuario);
            _logger.LogInformation($"Usuário com ID {usuario.Id} atualizado.");
        }

       
        public Usuario Criar(Usuario usuario)
        {

            // Inicializa a lista se estiver nula
            /* if (ListaUsuario.Usuarios == null)
             {
                 ListaUsuario.Usuarios = new List<Usuario>();
             }
             usuario.Id = ListaUsuario.Usuarios.Select(x => x.Id).Any() ? ListaUsuario.Usuarios.Select(x => x.Id).Max() + 1 : 1;
             ListaUsuario.Usuarios.Add(usuario);

             return usuario;*/

            var comandoSql = @"INSERT INTO USUARIO (UserName, Password, Ativo, PermisaoSistema) VALUES (@UserName, @Password, @Ativo, @PermisaoSistema);
                               SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = _dbConnection.Query<int>(comandoSql, usuario).Single();
            usuario.Id = id;
            _logger.LogInformation($"Usuário criado com ID {usuario.Id}.");
            return usuario;
        }
       
        public void Inativar(long id)
        {
            /* var usuario = ListaUsuario.Usuarios.FirstOrDefault(u => u.Id == id);
             if (usuario != null)
             {
                 usuario.Ativo = false;
             }*/
            var comandoSql = @"UPDATE USUARIO SET Ativo = 0 WHERE Id = @Id";
            _dbConnection.Execute(comandoSql, new { Id = id });
            _logger.LogInformation($"Usuário com ID {id} inativado.");
        }
       
        public IEnumerable<Usuario> Listar()
        {
            //return ListaUsuario.Usuarios;

            var comandoSql = @"SELECT * FROM USUARIO";
            _logger.LogInformation("Listando todos os usuários.");
            return _dbConnection.Query<Usuario>(comandoSql).ToList();

        }
       
        public Usuario Obter(long id)
        {
            // return ListaUsuario.Usuarios.FirstOrDefault(u => u.Id == id);

            var comandoSql = @"SELECT * FROM USUARIO where Id = @ID";
            _logger.LogInformation($"Obtendo usuário com ID {id}.");
            return _dbConnection.Query<Usuario>(comandoSql, new {ID = id}).SingleOrDefault();
        }
    }
}
