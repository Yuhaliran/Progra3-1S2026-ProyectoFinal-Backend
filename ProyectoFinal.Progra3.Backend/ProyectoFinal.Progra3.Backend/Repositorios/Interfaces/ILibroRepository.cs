namespace ProyectoFinal.Progra3.Backend.Repositorios.Interfaces
{
    using ProyectoFinal.Progra3.Backend.Modelos.Request.Libros;
    using ProyectoFinal.Progra3.Backend.Modelos.Response.Libros;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILibroRepository
    {

        Task<IEnumerable<LibroResponse>> ObtenerTodosAsync();

        Task<LibroResponse> ObtenerPorIdAsync(int id);


        Task<int> CrearAsync(CrearLibroRequest request);


        Task<bool> ActualizarAsync(int id, CrearLibroRequest request);


        Task<bool> EliminarAsync(int id);
    }
}