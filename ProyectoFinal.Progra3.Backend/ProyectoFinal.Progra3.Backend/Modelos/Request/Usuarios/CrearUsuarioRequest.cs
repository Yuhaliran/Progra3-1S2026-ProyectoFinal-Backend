namespace ProyectoFinal.Progra3.Backend.Modelos.Request.Usuarios
{
    public class CrearUsuarioRequest
    {
        public int IdRol { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? DPI { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
    }
}
