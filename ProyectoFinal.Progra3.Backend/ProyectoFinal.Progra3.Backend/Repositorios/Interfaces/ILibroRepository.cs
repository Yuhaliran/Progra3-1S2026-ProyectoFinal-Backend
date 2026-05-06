namespace ProyectoFinal.Progra3.Backend.Repositorios.Interfaces
{
    using ProyectoFinal.Progra3.Backend.Modelos.Request.Libros;
    using ProyectoFinal.Progra3.Backend.Modelos.Response.Libros;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILibroRepository
    {
        Task<IEnumerable<LibroResponse>> ObtenerTodosAsync();


        Task<LibroResponse> ObtenerPorIsbnAsync(string isbn);


        Task<string> CrearAsync(CrearLibroRequest request);


        Task<bool> ActualizarAsync(string isbn, CrearLibroRequest request);


        Task<bool> EliminarAsync(string isbn);
    }
}