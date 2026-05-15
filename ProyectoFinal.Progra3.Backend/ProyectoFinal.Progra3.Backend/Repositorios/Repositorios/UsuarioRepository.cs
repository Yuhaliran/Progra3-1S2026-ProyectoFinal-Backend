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

        public async Task<bool> ActualizarAsync(int id, EditarUsuarioRequest request)
        {
            // Consulta SQL para actualizar solo los campos definidos en tu modelo
            string sql = @" UPDATE dbo.Usuarios 
                               SET IdEstado = @IdEstado, 
                                   Nombres = @Nombres, 
                                   Apellidos = @Apellidos, 
                                   Telefono = @Telefono 
                             WHERE IdUsuario = @IdUsuario";

            // Emparejamos el ID que viene de la ruta con los datos del request
            var parametros = new
            {
                IdUsuario = id,
                request.IdEstado,
                request.Nombres,
                request.Apellidos,
                request.Telefono
            };

            // Ejecutamos la consulta y verificamos si se modificó al menos una fila
            var filasAfectadas = await _db.ExecuteAsync(sql, parametros);
            return filasAfectadas > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            // Consulta SQL para borrar el registro por su llave principal
            string sql = @" DELETE FROM dbo.Usuarios WHERE IdUsuario = @IdUsuario";

            var filasAfectadas = await _db.ExecuteAsync(sql, new { IdUsuario = id });
            return filasAfectadas > 0;
        }



    }
}
