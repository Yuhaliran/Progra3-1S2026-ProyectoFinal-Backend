namespace ProyectoFinal.Progra3.Backend.Repositorios
{
    using Dapper;
    using System.Data;
    using ProyectoFinal.Progra3.Backend.Repositorios.Interfaces;
    using ProyectoFinal.Progra3.Backend.Modelos.Request.Libros;
    using ProyectoFinal.Progra3.Backend.Modelos.Response.Libros;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class LibroRepository : ILibroRepository
    {
        // Variable para manejar la conexión a la base de datos
        private readonly IDbConnection _db;

        // El constructor recibe la conexión, igual que en Usuarios
        public LibroRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<LibroResponse>> ObtenerTodosAsync()
        {
            // Escribimos nuestra consulta SQL para traer todos los libros
            string sql = @" SELECT IdLibro, 
                                   Nombre, 
                                   Autor, 
                                   Genero, 
                                   Isbn, 
                                   FotoLibroUrl, 
                                   Sinopsis 
                              FROM dbo.Libros";

            // QueryAsync devuelve una lista de LibroResponse
            return await _db.QueryAsync<LibroResponse>(sql);
        }

        public async Task<LibroResponse> ObtenerPorIdAsync(int id)
        {
            // Consulta SQL con un filtro WHERE
            string sql = @" SELECT IdLibro, 
                                   Nombre, 
                                   Autor, 
                                   Genero, 
                                   Isbn, 
                                   FotoLibroUrl, 
                                   Sinopsis 
                              FROM dbo.Libros 
                             WHERE IdLibro = @IdLibro";

            // Pasamos el parámetro @IdLibro de forma segura
            return await _db.QueryFirstOrDefaultAsync<LibroResponse>(sql, new { IdLibro = id });
        }

        public async Task<int> CrearAsync(CrearLibroRequest request)
        {
            // Insertamos los datos y recuperamos el nuevo ID generado
            string sql = @" INSERT INTO dbo.Libros (
                                        Nombre, 
                                        Autor, 
                                        Genero, 
                                        Isbn, 
                                        FotoLibroUrl, 
                                        Sinopsis
                                   ) 
                            VALUES (
                                        @Nombre, 
                                        @Autor, 
                                        @Genero, 
                                        @Isbn, 
                                        @FotoLibroUrl, 
                                        @Sinopsis
                                   );
                            
                            SELECT CAST(SCOPE_IDENTITY() AS int);";

            // QuerySingleAsync ejecuta el INSERT y nos devuelve el ID que arrojó SCOPE_IDENTITY()
            return await _db.QuerySingleAsync<int>(sql, request);
        }

        public async Task<bool> ActualizarAsync(int id, CrearLibroRequest request)
        {
            // Consulta para actualizar los campos
            string sql = @" UPDATE dbo.Libros 
                               SET Nombre = @Nombre, 
                                   Autor = @Autor, 
                                   Genero = @Genero, 
                                   Isbn = @Isbn, 
                                   FotoLibroUrl = @FotoLibroUrl, 
                                   Sinopsis = @Sinopsis 
                             WHERE IdLibro = @IdLibro";

            // Preparamos los parámetros combinando el ID de la URL y los datos del request
            var parametros = new
            {
                IdLibro = id,
                request.Nombre,
                request.Autor,
                request.Genero,
                request.Isbn,
                request.FotoLibroUrl,
                request.Sinopsis
            };

            // ExecuteAsync devuelve el número de filas afectadas. Si es mayor a 0, fue exitoso.
            var filasAfectadas = await _db.ExecuteAsync(sql, parametros);
            return filasAfectadas > 0;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            string sql = @" DELETE FROM dbo.Libros WHERE IdLibro = @IdLibro";

            var filasAfectadas = await _db.ExecuteAsync(sql, new { IdLibro = id });
            return filasAfectadas > 0;
        }
    }
}