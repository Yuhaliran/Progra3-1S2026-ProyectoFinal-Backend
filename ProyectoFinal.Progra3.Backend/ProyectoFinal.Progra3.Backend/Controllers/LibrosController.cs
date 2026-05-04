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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroResponse>>> GetLibros()
        {
            var libros = await _libroRepository.ObtenerTodosAsync();
            return Ok(libros);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LibroResponse>> GetLibro(int id)
        {
            var libro = await _libroRepository.ObtenerPorIdAsync(id);

            if (libro == null)
            {
                return NotFound(new { mensaje = $"El libro con ID {id} no fue encontrado" });
            }

            return Ok(libro);
        }

        [HttpPost]
        public async Task<ActionResult<int>> PostLibro([FromBody] CrearLibroRequest request)
        {
            try
            {

                if (string.IsNullOrEmpty(request.Nombre))
                {
                    return BadRequest(new { mensaje = "El nombre del libro es obligatorio" });
                }

                int nuevoId = await _libroRepository.CrearAsync(request);

                return CreatedAtAction(nameof(GetLibro), new { id = nuevoId },
                                       new { id = nuevoId, mensaje = "Libro creado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al procesar el libro", detalle = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutLibro(int id, [FromBody] CrearLibroRequest request)
        {
            var actualizado = await _libroRepository.ActualizarAsync(id, request);

            if (!actualizado)
            {
                return NotFound(new { mensaje = $"No se pudo actualizar. El libro con ID {id} no existe." });
            }

            return Ok(new { mensaje = "Libro actualizado correctamente" });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLibro(int id)
        {
            var eliminado = await _libroRepository.EliminarAsync(id);

            if (!eliminado)
            {
                return NotFound(new { mensaje = $"No se pudo eliminar. El libro con ID {id} no existe." });
            }

            return Ok(new { mensaje = "Libro eliminado correctamente" });
        }
    }
}