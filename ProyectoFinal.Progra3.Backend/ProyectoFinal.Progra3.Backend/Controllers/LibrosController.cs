namespace ProyectoFinal.Progra3.Backend.Controladores
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ProyectoFinal.Progra3.Backend.Modelos.Libros;
    using ProyectoFinal.Progra3.Backend.Modelos.Request.Libros;
    using ProyectoFinal.Progra3.Backend.Modelos.Response.Libros;
    using ProyectoFinal.Progra3.Backend.Repositorios.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly ILibroRepository _libroRepository;

        public LibrosController(ILibroRepository libroRepository)
        {
            _libroRepository = libroRepository;
        }

        // Le agregamos el nombre "obtenerTodos" a la ruta
        [HttpGet("obtenerTodos")]
        public async Task<ActionResult<IEnumerable<LibroResponse>>> GetLibros()
        {
            var libros = await _libroRepository.ObtenerTodosAsync();
            return Ok(libros);
        }

        // Le agregamos el nombre "obtenerPorIsbn" y recibimos el ISBN como texto
        [HttpGet("obtenerPorIsbn/{isbn}")]
        public async Task<ActionResult<LibroResponse>> GetLibro(string isbn)
        {
            var libro = await _libroRepository.ObtenerPorIsbnAsync(isbn);

            if (libro == null)
            {
                return NotFound(new { mensaje = $"El libro con ISBN {isbn} no fue encontrado" });
            }

            return Ok(libro);
        }

        // Le agregamos el nombre "crear" a la ruta
        [HttpPost("crear")]
        public async Task<ActionResult<string>> PostLibro([FromBody] CrearLibroRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ISBN))
                {
                    return BadRequest(new { mensaje = "El ISBN del libro es obligatorio" });
                }

                string nuevoIsbn = await _libroRepository.CrearAsync(request);

                return CreatedAtAction(nameof(GetLibro), new { isbn = nuevoIsbn },
                                       new { isbn = nuevoIsbn, mensaje = "Libro creado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al procesar el libro", detalle = ex.Message });
            }
        }

        // Le agregamos el nombre "actualizar" y usamos el ISBN
        [HttpPut("actualizar/{isbn}")]
        public async Task<ActionResult> PutLibro(string isbn, [FromBody] CrearLibroRequest request)
        {
            var actualizado = await _libroRepository.ActualizarAsync(isbn, request);

            if (!actualizado)
            {
                return NotFound(new { mensaje = $"No se pudo actualizar. El libro con ISBN {isbn} no existe." });
            }

            return Ok(new { mensaje = "Libro actualizado correctamente" });
        }

        // Le agregamos el nombre "eliminar" y usamos el ISBN
        [HttpDelete("eliminar/{isbn}")]
        public async Task<ActionResult> DeleteLibro(string isbn)
        {
            var eliminado = await _libroRepository.EliminarAsync(isbn);

            if (!eliminado)
            {
                return NotFound(new { mensaje = $"No se pudo eliminar. El libro con ISBN {isbn} no existe." });
            }

            return Ok(new { mensaje = "Libro eliminado correctamente" });
        }

        [HttpGet("buscar-unificado/{isbn}")]
        public async Task<IActionResult> BuscarLibroUnificado(string isbn)
        {
    

            // Usamos tu repositorio para buscar el libro en la base de datos
            var libroLocal = await _libroRepository.ObtenerPorIsbnAsync(isbn);

            if (libroLocal != null)
            {
                // Si ya existe localmente, lo devolvemos inmediatamente
                var respuestaLocal = new LibroDto
                {
                    ISBN = libroLocal.ISBN,
                    Titulo = libroLocal.Titulo,
                    Autor = libroLocal.Autor,
                    AnioPublicacion = libroLocal.AnioPublicacion,
                    Portada = libroLocal.Portada
                };
                return Ok(respuestaLocal);
            }

            
           

            string url = $"https://openlibrary.org/search.json?isbn={isbn}";

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "XandriaBiblioteca/1.0");

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;

                // Verificamos si OpenLibrary encontró el libro
                if (root.TryGetProperty("numFound", out JsonElement numFound) && numFound.GetInt32() > 0)
                {
                    var bookNode = root.GetProperty("docs")[0];

                    // Extraemos la información
                    string titulo = bookNode.TryGetProperty("title", out var titleElement) ? titleElement.GetString() : "Sin Título";

                    string autor = "Autor Desconocido";
                    if (bookNode.TryGetProperty("author_name", out var authorsArray) && authorsArray.GetArrayLength() > 0)
                    {
                        autor = authorsArray[0].GetString();
                    }

                    int anioFiltrado = 0;
                    if (bookNode.TryGetProperty("first_publish_year", out var yearElement) && yearElement.ValueKind == JsonValueKind.Number)
                    {
                        anioFiltrado = yearElement.GetInt32();
                    }

                   
                    string portadaUrl = $"https://covers.openlibrary.org/b/isbn/{isbn}-L.jpg";
                    string portadaBase64 = "Sin Portada";

                    try
                    {
                        byte[] imageBytes = await client.GetByteArrayAsync(portadaUrl);
                        portadaBase64 = "data:image/jpeg;base64," + Convert.ToBase64String(imageBytes);
                    }
                    catch
                    {
                        // Si la imagen falla o no existe, se queda con el texto "Sin Portada"
                    }

                

                    // Creamos el objeto exactamente como lo pide tu interfaz ILibroRepository
                    var nuevoLibroRequest = new CrearLibroRequest
                    {
                        ISBN = isbn,
                        Titulo = titulo,
                        Autor = autor,
                        AnioPublicacion = anioFiltrado,
                        Portada = portadaBase64
                    };

                    // Ejecutamos la inserción en la base de datos local
                    await _libroRepository.CrearAsync(nuevoLibroRequest);


                    var libroExterno = new LibroDto
                    {
                        ISBN = nuevoLibroRequest.ISBN,
                        Titulo = nuevoLibroRequest.Titulo,
                        Autor = nuevoLibroRequest.Autor,
                        AnioPublicacion = nuevoLibroRequest.AnioPublicacion,
                        Portada = nuevoLibroRequest.Portada
                    };

                    return Ok(libroExterno);
                }
                else
                {
                    return NotFound(new { mensaje = $"OpenLibrary no encontró ningún libro con el código: {isbn}." });
                }
            }

            return StatusCode(500, new { mensaje = "Error al intentar conectar con OpenLibrary." });
        }

        [HttpGet("buscar-local")]
        public async Task<ActionResult<IEnumerable<LibroResponse>>> BuscarLocal([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Ok(new List<LibroResponse>());
            }
            var libros = await _libroRepository.BuscarPorTituloOAutorAsync(query);
            return Ok(libros);
        }
    }
}