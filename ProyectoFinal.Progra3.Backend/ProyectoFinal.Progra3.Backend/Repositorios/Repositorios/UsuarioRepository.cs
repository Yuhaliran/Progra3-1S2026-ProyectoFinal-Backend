namespace ProyectoFinal.Progra3.Backend.Repositorios.Repositorios
{
    using Dapper;
    using System.Data;
    using ProyectoFinal.Progra3.Backend.Modelos.Response.Auth;
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
                                   u.DPI,
                                   u.Email,
                                   u.Telefono,
                                   r.NombreRol AS Rol,
                                   CASE WHEN u.IdEstado = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
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
                                                  Telefono,
                                                  PasswordHash
                                                 )
                                          VALUES (
                                                  @IdRol,
                                                  1,
                                                  @Nombres,
                                                  @Apellidos,
                                                  @DPI,
                                                  @Email,
                                                  @Telefono,
                                                  @PasswordHash
                                                 );
                
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            var parametros = new {
                request.IdRol,
                request.Nombres,
                request.Apellidos,
                request.DPI,
                request.Email,
                request.Telefono,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            return await _db.QuerySingleAsync<int>(sql, parametros);
        }

        public async Task<IEnumerable<UsuarioResponse>> ObtenerTodosAsync()
        {
            string sql = @" SELECT u.IdUsuario,
                                   u.Nombres,
                                   u.Apellidos,
                                   r.NombreRol AS Rol
                              FROM dbo.Usuarios u
                                   INNER JOIN Roles r ON u.IdRol = r.IdRol";

            return await _db.QueryAsync<UsuarioResponse>(sql);
        }

        public async Task<bool> ActualizarAsync(int id, EditarUsuarioRequest request)
        {
            string sql = @" UPDATE dbo.Usuarios
                               SET Nombres = @Nombres,
                                   Apellidos = @Apellidos,
                                   Telefono = @Telefono,
                                   IdEstado = @IdEstado
                             WHERE IdUsuario = @IdUsuario";

            var parametros = new {
                request.Nombres,
                request.Apellidos,
                request.Telefono,
                request.IdEstado,
                IdUsuario = id
            };

            int filasAfectadas = await _db.ExecuteAsync(sql, parametros);
            return filasAfectadas > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            // Usaremos borrado lógico cambiando el IdEstado a 0 (Inactivo) en lugar de un DELETE físico
            string sql = @" UPDATE dbo.Usuarios SET IdEstado = 0 WHERE IdUsuario = @IdUsuario";
            
            int filasAfectadas = await _db.ExecuteAsync(sql, new { IdUsuario = id });
            return filasAfectadas > 0;
        }

        public async Task<UsuarioAuthModel?> ObtenerPorEmailParaAuthAsync(string email)
        {
            string sql = @" SELECT u.IdUsuario,
                                   u.Email,
                                   u.PasswordHash,
                                   u.IdRol,
                                   r.NombreRol AS Rol
                              FROM dbo.Usuarios u
                                   INNER JOIN Roles r ON u.IdRol = r.IdRol
                             WHERE u.Email = @Email AND u.IdEstado = 1"; // Solo usuarios activos

            return await _db.QueryFirstOrDefaultAsync<UsuarioAuthModel>(sql, new { Email = email });
        }
    }
}
