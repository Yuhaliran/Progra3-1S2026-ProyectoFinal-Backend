namespace ProyectoFinal.Progra3.Backend.Repositorios.Interfaces
{
    using ProyectoFinal.Progra3.Backend.Modelos.Request.Usuarios;
    using ProyectoFinal.Progra3.Backend.Modelos.Response.Auth;
    using ProyectoFinal.Progra3.Backend.Modelos.Response.Usuarios;

    public interface IUsuarioRepository
    {
        Task<IEnumerable<UsuarioResponse>> ObtenerTodosAsync();

        Task<UsuarioResponse> ObtenerPorIdAsync(int id);

        Task<int> CrearAsync(CrearUsuarioRequest request);

        Task<bool> ActualizarAsync(int id, EditarUsuarioRequest request);

        Task<bool> EliminarAsync(int id);

        Task<UsuarioAuthModel?> ObtenerPorEmailParaAuthAsync(string email);
    }
}
