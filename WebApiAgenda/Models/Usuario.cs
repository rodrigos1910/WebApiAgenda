using System.ComponentModel.DataAnnotations;

using WebApiAgenda.Enums;

namespace WebApiAgenda.Models
{
    //simular lista na memoria
    public static class ListaUsuario
    {
        public static IList<Usuario> Usuarios { get; set; }
    }

    public class Usuario
    {      

        public int Id { get; set; }

        [Required(ErrorMessage = "Usuario é obrigatório")]
        [StringLength(100, ErrorMessage = "Usuario pode ter no máximo 100 caracteres")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password é obrigatório")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "A senha deve ter pelo menos uma letra maiúscula, um caracter especial, um número e no mínimo 8 caracteres")]
        public string Password { get; set; }

        public bool Ativo { get; set; }

        [Required(ErrorMessage = "PermisaoSistema é obrigatório")]
        public TipoPermisaoSistema PermisaoSistema { get; set; }


    }
}
