namespace ProyectoFinal.Progra3.Backend.Controladores
{
    using Microsoft.AspNetCore.Mvc;
    using ProyectoFinal.Progra3.Backend.Modelos.Request.Libros;
    using ProyectoFinal.Progra3.Backend.Modelos.Response.Libros;
    using ProyectoFinal.Progra3.Backend.Repositorios.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
    }
}