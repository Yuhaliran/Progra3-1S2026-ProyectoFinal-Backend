namespace ProyectoFinal.Progra3.Backend.Repositorios.Repositorios
{
    using Dapper;
    using System.Data;
    using ProyectoFinal.Progra3.Backend.Repositorios.Interfaces;
    using ProyectoFinal.Progra3.Backend.Modelos.Request.Usuarios;
    using ProyectoFinal.Progra3.Backend.Modelos.Response.Usuarios;

    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IDbConnection _db;

        public UsuarioRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<UsuarioResponse> ObtenerPorIdAsync(int id)
        {
            string sql = @" SELECT u.IdUsuario,
                                   u.Nombres,
                                   u.Apellidos,
                                   r.NombreRol AS Rol
                              FROM dbo.Usuarios u
                                   INNER JOIN Roles r ON u.IdRol = r.IdRol
                             WHERE u.IdUsuario = @IdUsuario";

            return await _db.QueryFirstOrDefaultAsync<UsuarioResponse>(sql, new { IdUsuario = id });
        }

        public async Task<int> CrearAsync(CrearUsuarioRequest request)
        {
            string sql = @" INSERT INTO Usuarios (
                                                  IdRol,
                                                  IdEstado,
                                                  Nombres,
                                                  Apellidos,
                                                  DPI,
                                                  Email,
                                                  Telefono
                                                 )
                                          VALUES (
                                                  @IdRol,
                                                  1,
                                                  @Nombres,
                                                  @Apellidos,
                                                  @DPI,
                                                  @Email,
                                                  @Telefono
                                                 );
                
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            return await _db.QuerySingleAsync<int>(sql, request);
        }
    }
}
