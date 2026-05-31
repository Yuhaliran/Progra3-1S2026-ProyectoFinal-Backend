namespace ProyectoFinal.Progra3.Backend.Repositorios.Repositorios
{
    using Dapper;
    using System.Data;
    using ProyectoFinal.Progra3.Backend.Modelos;
    using ProyectoFinal.Progra3.Backend.Modelos.Request.Bitacora;
    using ProyectoFinal.Progra3.Backend.Modelos.Response.Bitacora;
    using ProyectoFinal.Progra3.Backend.Repositorios.Interfaces;

    public class ColaLecturaRepository : IColaLecturaRepository
    {
        private readonly IDbConnection _db;

        public ColaLecturaRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ColaLecturaResponse>> ObtenerColaPorUsuarioAsync(int idUsuario)
        {
            string sql = @"
                SELECT c.IdColaLectura, c.IdUsuario, c.ISBN, c.IdEstadoLectura, c.FechaInicio, c.FechaFin, c.MeGusto,
                       e.NombreEstado, l.Titulo AS TituloLibro, l.Autor AS AutorLibro, c.FechaCreacion
                FROM ColaLectura c
                INNER JOIN EstadosLectura e ON c.IdEstadoLectura = e.IdEstadoLectura
                INNER JOIN Libros l ON c.ISBN = l.ISBN
                WHERE c.IdUsuario = @IdUsuario
                ORDER BY c.FechaCreacion ASC
            ";
            return await _db.QueryAsync<ColaLecturaResponse>(sql, new { IdUsuario = idUsuario });
        }

        public async Task<ColaLecturaResponse?> ObtenerRegistroColaAsync(int idUsuario, string isbn)
        {
            string sql = @"
                SELECT c.IdColaLectura, c.IdUsuario, c.ISBN, c.IdEstadoLectura, c.FechaInicio, c.FechaFin, c.MeGusto,
                       e.NombreEstado, l.Titulo AS TituloLibro, l.Autor AS AutorLibro, c.FechaCreacion
                FROM ColaLectura c
                INNER JOIN EstadosLectura e ON c.IdEstadoLectura = e.IdEstadoLectura
                INNER JOIN Libros l ON c.ISBN = l.ISBN
                WHERE c.IdUsuario = @IdUsuario AND c.ISBN = @ISBN
            ";
            return await _db.QueryFirstOrDefaultAsync<ColaLecturaResponse>(sql, new { IdUsuario = idUsuario, ISBN = isbn });
        }

        public async Task<int> AgregarOActualizarRegistroColaAsync(int idUsuario, ColaLecturaRequest request)
        {
            string sql = @"
                MERGE ColaLectura AS target
                USING (SELECT @IdUsuario AS IdUsuario, @ISBN AS ISBN) AS source
                ON (target.IdUsuario = source.IdUsuario AND target.ISBN = source.ISBN)
                WHEN MATCHED THEN
                    UPDATE SET IdEstadoLectura = @IdEstadoLectura, 
                               FechaInicio = @FechaInicio, 
                               FechaFin = @FechaFin, 
                               MeGusto = @MeGusto
                WHEN NOT MATCHED THEN
                    INSERT (IdUsuario, ISBN, IdEstadoLectura, FechaInicio, FechaFin, MeGusto)
                    VALUES (@IdUsuario, @ISBN, @IdEstadoLectura, @FechaInicio, @FechaFin, @MeGusto);
            ";
            
            var param = new {
                IdUsuario = idUsuario,
                ISBN = request.ISBN,
                IdEstadoLectura = request.IdEstadoLectura,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                MeGusto = request.MeGusto
            };

            return await _db.ExecuteAsync(sql, param);
        }

        public async Task<IEnumerable<EstadoLectura>> ObtenerEstadosLecturaAsync()
        {
            string sql = "SELECT IdEstadoLectura, NombreEstado FROM EstadosLectura";
            return await _db.QueryAsync<EstadoLectura>(sql);
        }

        public async Task<bool> EliminarRegistroColaAsync(int idUsuario, string isbn)
        {
            string sql = "DELETE FROM dbo.ColaLectura WHERE IdUsuario = @IdUsuario AND ISBN = @ISBN";
            int filasAfectadas = await _db.ExecuteAsync(sql, new { IdUsuario = idUsuario, ISBN = isbn });
            return filasAfectadas > 0;
        }
    }
}
