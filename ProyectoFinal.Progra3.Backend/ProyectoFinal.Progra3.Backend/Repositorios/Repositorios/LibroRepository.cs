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
        private readonly IDbConnection _db;

        public LibroRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<LibroResponse>> ObtenerTodosAsync()
        {
            // Agregamos Portada al SELECT
            string sql = @" SELECT ISBN, Titulo, Autor, AnioPublicacion, Portada FROM dbo.Libros";
            return await _db.QueryAsync<LibroResponse>(sql);
        }

        public async Task<LibroResponse> ObtenerPorIsbnAsync(string isbn)
        {
            // Agregamos Portada al SELECT
            string sql = @" SELECT ISBN, Titulo, Autor, AnioPublicacion, Portada FROM dbo.Libros WHERE ISBN = @ISBN";
            return await _db.QueryFirstOrDefaultAsync<LibroResponse>(sql, new { ISBN = isbn });
        }

        public async Task<string> CrearAsync(CrearLibroRequest request)
        {
            // Agregamos Portada e @Portada al INSERT
            string sql = @" INSERT INTO dbo.Libros (ISBN, Titulo, Autor, AnioPublicacion, Portada) 
                            VALUES (@ISBN, @Titulo, @Autor, @AnioPublicacion, @Portada);";

            await _db.ExecuteAsync(sql, request);
            return request.ISBN;
        }

        public async Task<bool> ActualizarAsync(string isbn, CrearLibroRequest request)
        {
            // Agregamos Portada = @Portada al UPDATE
            string sql = @" UPDATE dbo.Libros 
                               SET Titulo = @Titulo, 
                                   Autor = @Autor, 
                                   AnioPublicacion = @AnioPublicacion,
                                   Portada = @Portada 
                             WHERE ISBN = @ISBNBusqueda";

            var parametros = new
            {
                ISBNBusqueda = isbn,
                request.Titulo,
                request.Autor,
                request.AnioPublicacion,
                request.Portada
            };

            var filasAfectadas = await _db.ExecuteAsync(sql, parametros);
            return filasAfectadas > 0;
        }

        public async Task<bool> EliminarAsync(string isbn)
        {
            string sql = @" DELETE FROM dbo.Libros WHERE ISBN = @ISBN";
            var filasAfectadas = await _db.ExecuteAsync(sql, new { ISBN = isbn });
            return filasAfectadas > 0;
        }

        public async Task<IEnumerable<LibroResponse>> BuscarPorTituloOAutorAsync(string query)
        {
            string sql = @" SELECT ISBN, Titulo, Autor, AnioPublicacion, Portada 
                              FROM dbo.Libros 
                             WHERE Titulo LIKE @Query OR Autor LIKE @Query";
            return await _db.QueryAsync<LibroResponse>(sql, new { Query = "%" + query + "%" });
        }
    }
}