using Bogus;

using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApiAgenda.Controllers;

using WebApiAgenda.Interfaces;
using WebApiAgenda.Models;

using Xunit;

namespace WebApiAgenda.Tests.Controllers
{
    public class ContatosControllersTests
    {
        private readonly Mock<IContatoRepository> _mockRepo;
        private readonly Mock<ILogger<ContatosController>> _mockLogger;
        private readonly ContatosController _controller;
        private readonly Faker<Contato> _fakerContato;
        public ContatosControllersTests()
        {
            _mockRepo = new Mock<IContatoRepository>();
            _mockLogger = new Mock<ILogger<ContatosController>>();
            _controller = new ContatosController(_mockRepo.Object, _mockLogger.Object);

            // Configurar o Bogus para gerar contatos falsos
            _fakerContato = new Faker<Contato>()
                .RuleFor(c => c.Id, f => f.IndexFaker + 1)
                .RuleFor(c => c.Nome, f => f.Name.FullName())
                .RuleFor(c => c.Ddd, f => f.PickRandom(new[] { "11", "21", "31" }))
                .RuleFor(c => c.Telefone, f => f.Phone.PhoneNumber("#########"))
                .RuleFor(c => c.Email, f => f.Internet.Email());
        }



        [Fact(DisplayName = "Validando Consulta de Contatos")]
        [Trait("Contato Controler","Validando Contatos")]
        public void GetContatos_ReturnsOkResult_WithListOfContacts()
        {
            // Arrange
            var contatos = _fakerContato.Generate(3);
            _mockRepo.Setup(repo => repo.Listar(It.IsAny<string>())).Returns(contatos);

            // Act
            var result = _controller.GetContatos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Contato>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
        }


        [Fact(DisplayName = "Validando Consulta de um Contato")]
        [Trait("Contato Controler", "Validando Contatos")]
        public void GetContato_ReturnsOkResult_WithContact()
        {
            // Arrange
            var contato = _fakerContato.Generate();
            _mockRepo.Setup(repo => repo.Obter(contato.Id)).Returns(contato);

            // Act
            var result = _controller.GetContato(contato.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Contato>(okResult.Value);
            Assert.Equal(contato.Id, returnValue.Id);
        }

        [Fact(DisplayName = "Validando Consulta de Contato não encontrado")]
        [Trait("Contato Controler", "Validando Contatos")]
        public void GetContato_ReturnsNotFound_WhenContactDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Obter(It.IsAny<long>())).Returns((Contato)null);

            // Act
            var result = _controller.GetContato(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact(DisplayName = "Validando Cadastro de Contato Valido")]
        [Trait("Contato Controler", "Validando Contatos")]
        public void Criar_ReturnsCreatedAtActionResult_WhenContactIsValid()
        {
            // Arrange
            var contato = _fakerContato.Generate();
            _mockRepo.Setup(repo => repo.Criar(It.IsAny<Contato>())).Returns(contato);

            // Act
            var result = _controller.Criar(contato);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Contato>(createdAtActionResult.Value);
            Assert.Equal(contato.Id, returnValue.Id);
        }

        [Fact(DisplayName = "Validando Cadastro de Contato Não Preenchido")]
        [Trait("Contato Controler", "Validando Contatos")]
        public void Criar_ReturnsBadRequest_WhenContactIsNull()
        {
            // Act
            var result = _controller.Criar(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact(DisplayName = "Validando Atualização de Contato Valido")]
        [Trait("Contato Controler", "Validando Contatos")]
        public void Atualizar_ReturnsOkResult_WhenContactIsUpdated()
        {
            // Arrange
            var contato = _fakerContato.Generate();
            _mockRepo.Setup(repo => repo.Obter(contato.Id)).Returns(contato);
            _mockRepo.Setup(repo => repo.Atualizar(contato.Id, contato)).Verifiable();

            // Act
            var result = _controller.Atualizar(contato.Id, contato);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockRepo.Verify();
        }

        [Fact(DisplayName = "Validando Atualizacao de Contato não encontrado")]
        [Trait("Contato Controler", "Validando Contatos")]
        public void Atualizar_ReturnsNotFound_WhenContactDoesNotExist()
        {
            // Arrange
            var contato = _fakerContato.Generate();
            _mockRepo.Setup(repo => repo.Obter(contato.Id)).Returns((Contato)null);

            // Act
            var result = _controller.Atualizar(contato.Id, contato);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact(DisplayName = "Validando Exclusão de Contato")]
        [Trait("Contato Controler", "Validando Contatos")]
        public void Remover_ReturnsOkResult_WhenContactIsDeleted()
        {
            // Arrange
            var contato = _fakerContato.Generate();
            _mockRepo.Setup(repo => repo.Obter(contato.Id)).Returns(contato);
            _mockRepo.Setup(repo => repo.Deletar(contato.Id)).Verifiable();

            // Act
            var result = _controller.Remover(contato.Id);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockRepo.Verify();
        }

        [Fact(DisplayName = "Validando Exclusão de Contato não encontrado")]
        [Trait("Contato Controler", "Validando Contatos")]
        public void Remover_ReturnsNotFound_WhenContactDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Obter(It.IsAny<long>())).Returns((Contato)null);

            // Act
            var result = _controller.Remover(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
