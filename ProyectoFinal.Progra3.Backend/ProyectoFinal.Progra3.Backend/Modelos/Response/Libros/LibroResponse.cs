namespace ProyectoFinal.Progra3.Backend.Modelos.Response.Libros
{
    public class LibroResponse
    {
        public string ISBN { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int? AnioPublicacion { get; set; }
    }
}