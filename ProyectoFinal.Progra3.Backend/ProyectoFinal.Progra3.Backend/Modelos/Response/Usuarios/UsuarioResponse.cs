namespace ProyectoFinal.Progra3.Backend.Modelos.Response.Usuarios
{
    public class UsuarioResponse
    {
        public int IdUsuario { get; set; }
        public string Rol { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string DPI { get; set; } = string.Empty;
        public string IdentificadorBiblioteca { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
    }
}
