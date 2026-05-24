namespace ProyectoFinal.Progra3.Backend.Modelos.Request.Libros
{
    public class CrearLibroRequest
    {
        public string ISBN { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int? AnioPublicacion { get; set; }

        public string Portada { get; set; } = string.Empty;
    }
}