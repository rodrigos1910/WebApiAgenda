using Bogus;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

using WebApiAgenda.Models;

using Xunit;

namespace WebApiAgenda.Tests.Models
{
    public class ContatoTests
    {
        private readonly Faker<Contato> _fakerContato;
        public ContatoTests() {

            // Configura o Bogus para gerar contatos falsos
            _fakerContato = new Faker<Contato>()
                .RuleFor(c => c.Id, f => f.IndexFaker + 1)
                .RuleFor(c => c.Nome, f => f.Name.FullName())
                .RuleFor(c => c.Ddd, f => f.PickRandom(new[] { "11", "21", "31" }))
                .RuleFor(c => c.Telefone, f => f.Phone.PhoneNumber("#########"))
                .RuleFor(c => c.Email, f => f.Internet.Email());

        }


        private List<System.ComponentModel.DataAnnotations.ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }

        [Fact(DisplayName = "Validando Nome null de Contatos")]
        [Trait("Contato Entity", "Validando Contatos")]
        public void ValidateEntity_ContactName_IsRequired()
        {
            // Arrange
            var contato = _fakerContato.Generate();
            contato.Nome = null;

            // Act
            var results = ValidateModel(contato);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Nome") && v.ErrorMessage.Contains("Nome é obrigatório"));
        }


        [Fact(DisplayName = "Validando Nome muito grande de Contatos")]
        [Trait("Contato Entity", "Validando Contatos")]
        public void ValidateEntity_Nome_HasMaxLength()
        {
            // Arrange
            var contato = _fakerContato.Generate();
            contato.Nome = new string('a', 101); 

            // Act
            var results = ValidateModel(contato);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Nome") && v.ErrorMessage.Contains("Nome pode ter no máximo 100 caracteres"));
        }

        [Fact(DisplayName = "Validando Ddd null de Contatos")]
        [Trait("Contato Entity", "Validando Contatos")]
        public void ValidateEntity_Ddd_IsRequired()
        {
            // Arrange
            var contato = _fakerContato.Generate();
            contato.Ddd = null;

            // Act
            var results = ValidateModel(contato);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Ddd") && v.ErrorMessage.Contains("DDD é obrigatório"));
        }

        [Fact(DisplayName = "Validando Nome incorreto de Contatos")]
        [Trait("Contato Entity", "Validando Contatos")]
        public void ValidateEntity_Ddd_HasValidFormat()
        {
            // Arrange
            var contato = _fakerContato.Generate();
            contato.Ddd = "123"; 

            // Act
            var results = ValidateModel(contato);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Ddd") && v.ErrorMessage.Contains("DDD deve ter 2 dígitos"));
        }

        [Fact(DisplayName = "Validando Telefone null de Contatos")]
        [Trait("Contato Entity", "Validando Contatos")]
        public void ValidateEntity_Telefone_IsRequired()
        {
            // Arrange
            var contato = _fakerContato.Generate();
            contato.Telefone = null;

            // Act
            var results = ValidateModel(contato);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Telefone") && v.ErrorMessage.Contains("Telefone é obrigatório"));
        }

        [Fact(DisplayName = "Validando Nome incorreto de Contatos")]
        [Trait("Contato Entity", "Validando Contatos")]
        public void ValidateEntity_Telefone_HasValidFormat()
        {
            // Arrange
            var contato = _fakerContato.Generate(); 
            contato.Telefone = "123"; 

            // Act
            var results = ValidateModel(contato);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Telefone") && v.ErrorMessage.Contains("Telefone deve ter 8 ou 9 dígitos"));
        }

        [Fact(DisplayName = "Validando Email null de Contatos")]
        [Trait("Contato Entity", "Validando Contatos")]
        public void ValidateEntity_Email_IsRequired()
        {
            // Arrange
            var contato = _fakerContato.Generate();
            contato.Email = null;

            // Act
            var results = ValidateModel(contato);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Email") && v.ErrorMessage.Contains("Email é obrigatório"));
        }

        [Fact(DisplayName = "Validando E-mail incorreto de Contatos")]
        [Trait("Contato Entity", "Validando Contatos")]
        public void ValidateEntity_Email_HasValidFormat()
        {
            // Arrange
            var contato = _fakerContato.Generate();
            contato.Email = "posfiapinvalido.com"; 

            // Act
            var results = ValidateModel(contato);

            // Assert
            Assert.Contains(results, v => v.MemberNames.Contains("Email") && v.ErrorMessage.Contains("Email inválido"));
        }
    }
}
