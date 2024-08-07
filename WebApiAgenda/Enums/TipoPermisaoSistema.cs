namespace WebApiAgenda.Enums
{
    public enum TipoPermisaoSistema
    {
        Administrador,
        UsuarioComum,
        Supervisor,
        Convidado
    }

    public static class PermissaoSistema
    {
        public const string Administrador = "Administrador";
        public const string UsuarioComum = "UsuarioComum";
        public const string Supervisor = "Supervisor";
        public const string Convidado = "Convidado";
    }
   
}
