namespace ProyectoFinal.Progra3.Backend.Modelos.Response.Libros
{
    public class LibroResponse
    {
        public int IdLibro { get; set; } // El ID único de la base de datos
        public string Nombre { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;
        public string FotoLibroUrl { get; set; } = string.Empty;
        public string Sinopsis { get; set; } = string.Empty;
    }
}