using System.ComponentModel.DataAnnotations;

namespace WebApiAgenda.Models
{
    public class Contato
    {
       
        public long Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome pode ter no máximo 100 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "DDD é obrigatório")]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "DDD deve ter 2 dígitos")]
        public string Ddd { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [RegularExpression(@"^\d{8,9}$", ErrorMessage = "Telefone deve ter 8 ou 9 dígitos")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

    }
}
