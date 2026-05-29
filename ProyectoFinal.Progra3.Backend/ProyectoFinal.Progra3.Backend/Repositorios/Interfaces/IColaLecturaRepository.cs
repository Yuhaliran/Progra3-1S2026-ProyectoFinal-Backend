namespace ProyectoFinal.Progra3.Backend.Repositorios.Interfaces
{
    using ProyectoFinal.Progra3.Backend.Modelos;
    using ProyectoFinal.Progra3.Backend.Modelos.Request.Bitacora;
    using ProyectoFinal.Progra3.Backend.Modelos.Response.Bitacora;

    public interface IColaLecturaRepository
    {
        Task<IEnumerable<ColaLecturaResponse>> ObtenerColaPorUsuarioAsync(int idUsuario);
        Task<ColaLecturaResponse?> ObtenerRegistroColaAsync(int idUsuario, string isbn);
        Task<int> AgregarOActualizarRegistroColaAsync(int idUsuario, ColaLecturaRequest request);
        Task<IEnumerable<EstadoLectura>> ObtenerEstadosLecturaAsync();
    }
}
