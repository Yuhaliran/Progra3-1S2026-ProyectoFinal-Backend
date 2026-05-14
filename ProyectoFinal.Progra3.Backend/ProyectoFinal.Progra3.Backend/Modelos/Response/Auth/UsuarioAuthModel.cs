namespace ProyectoFinal.Progra3.Backend.Modelos.Response.Auth
{
    public class UsuarioAuthModel
    {
        public int IdUsuario { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public string Rol { get; set; } = string.Empty;
    }
}
