namespace ProyectoFinal.Progra3.Backend.Modelos.Request.Usuarios
{
    public class EditarUsuarioRequest
    {
        public int IdEstado { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? Telefono { get; set; }
    }
}
