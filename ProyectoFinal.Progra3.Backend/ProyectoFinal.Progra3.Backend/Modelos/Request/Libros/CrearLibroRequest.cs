namespace ProyectoFinal.Progra3.Backend.Modelos.Request.Libros
{
    public class CrearLibroRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string Isbn { get; set; } = string.Empty;
        public string FotoLibroUrl { get; set; } = string.Empty; // Guardaremos la ruta o URL de la foto
        public string Sinopsis { get; set; } = string.Empty;
    }
}