using Dapper;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System.Data;
using System.Data.SqlClient;

using WebApiAgenda.Models;
using WebApiAgenda.Repository;

using Xunit;

namespace WebApiAgenda.Tests.Integration.BD;

public class ContatoRepositoryTests : IDisposable
{
    private readonly IDbConnection _dbConnection;
    private readonly ContatoRepository _repository;

    public ContatoRepositoryTests()
    {
        // Carregar as configurações do appsettings.Test.json ou definir a conexão direto aqui
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        _dbConnection = new SqlConnection(connectionString);

        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<ContatoRepository>();

        _repository = new ContatoRepository(_dbConnection, logger);

        // Limpar a tabela antes de cada teste
        _dbConnection.Execute("TRUNCATE TABLE CONTATO");
    }

    [Fact(DisplayName = "Criar Contato - Deve Retornar Contato com ID")]
    [Trait("Contato", "Inserção")]
    public void CriarContato_DeveRetornarContatoComId()
    {
        // Arrange
        var contato = new Contato { Nome = "Teste", Email = "teste@teste.com", Telefone = "12345678", Ddd = "11" };

        // Act
        var contatoCriado = _repository.Criar(contato);

        // Assert
        Assert.NotNull(contatoCriado);
        Assert.True(contatoCriado.Id > 0);
    }

    [Fact(DisplayName = "Atualizar Contato - Deve Modificar Dados Corretamente")]
    [Trait("Contato", "Atualização")]
    public void AtualizarContato_DeveModificarDadosCorretamente()
    {
        // Arrange
        var contato = _repository.Criar(new Contato { Nome = "Teste", Email = "teste@teste.com", Telefone = "12345678", Ddd = "11" });
        contato.Nome = "Nome Atualizado";
        contato.Email = "atualizado@teste.com";

        // Act
        _repository.Atualizar(contato.Id, contato);
        var contatoAtualizado = _repository.Obter(contato.Id);

        // Assert
        Assert.NotNull(contatoAtualizado);
        Assert.Equal("Nome Atualizado", contatoAtualizado.Nome);
        Assert.Equal("atualizado@teste.com", contatoAtualizado.Email);
    }

    [Fact(DisplayName = "Deletar Contato - Deve Remover o Contato")]
    [Trait("Contato", "Deletar")]
    public void DeletarContato_DeveRemoverContato()
    {
        // Arrange
        var contato = _repository.Criar(new Contato { Nome = "Teste", Email = "teste@teste.com", Telefone = "12345678", Ddd = "11" });

        // Act
        _repository.Deletar(contato.Id);
        var contatoDeletado = _repository.Obter(contato.Id);

        // Assert
        Assert.Null(contatoDeletado);
    }

    [Fact(DisplayName = "Listar Contatos - Deve Retornar Todos os Contatos")]
    [Trait("Contato", "Listagem")]
    public void ListarContatos_DeveRetornarTodosContatos()
    {
        // Arrange
        var contato1 = _repository.Criar(new Contato { Nome = "Teste1", Email = "teste1@teste.com", Telefone = "12345678", Ddd = "11" });
        var contato2 = _repository.Criar(new Contato { Nome = "Teste2", Email = "teste2@teste.com", Telefone = "87654321", Ddd = "11" });

        // Act
        var contatos = _repository.Listar();

        // Assert
        Assert.NotNull(contatos);
        Assert.Equal(2, contatos.Count());
    }

    [Fact(DisplayName = "Obter Contato - Deve Retornar Contato por ID")]
    [Trait("Contato", "Obter")]
    public void ObterContato_DeveRetornarContatoPorId()
    {
        // Arrange
        var contato = _repository.Criar(new Contato { Nome = "Teste", Email = "teste@teste.com", Telefone = "12345678", Ddd = "11" });

        // Act
        var contatoObtido = _repository.Obter(contato.Id);

        // Assert
        Assert.NotNull(contatoObtido);
        Assert.Equal(contato.Nome, contatoObtido.Nome);
        Assert.Equal(contato.Email, contatoObtido.Email);
    }


    [Fact(DisplayName = "Atualizar Contato Inexistente - Deve Lançar Exceção")]
    [Trait("Contato", "Atualização")]
    public void AtualizarContato_Inexistente_DeveLancarExcecao()
    {
        // Arrange
        // Arrange
        var contatoInexistente = new Contato { Id = 9999, Nome = "Inexistente", Email = "inexistente@teste.com" };

        // Act
        var resultado = _repository.Atualizar(contatoInexistente.Id, contatoInexistente);

        // Assert
        Assert.False(resultado);
    }



    public void Dispose()
    {
       //  Limpa o banco de dados após cada teste
        _dbConnection.Execute("TRUNCATE TABLE CONTATO");
        _dbConnection.Dispose();
    }
}
