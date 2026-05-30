namespace ProyectoFinal.Progra3.Backend.Modelos.Libros
{
    public class LibroDto
    {
        public string ISBN { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int? AnioPublicacion { get; set; }
        public string Portada { get; set; }
    }
}