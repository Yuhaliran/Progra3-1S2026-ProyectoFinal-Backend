namespace ProyectoFinal.Progra3.Backend.Controladores
{
    using Microsoft.AspNetCore.Mvc;
    using ProyectoFinal.Progra3.Backend.Modelos.Request.Usuarios;
    using ProyectoFinal.Progra3.Backend.Modelos.Response.Usuarios;
    using ProyectoFinal.Progra3.Backend.Repositorios.Interfaces;


    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuariosController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet("obtenerTodos")]
        public async Task<ActionResult<IEnumerable<UsuarioResponse>>> GetUsuarios()
        {
            var usuarios = await _usuarioRepository.ObtenerTodosAsync();
            return Ok(usuarios);
        }

        [HttpGet("obtenerPorId/{id}")]
        public async Task<ActionResult<UsuarioResponse>> GetUsuario(int id)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);

            if (usuario == null)
            {
                return NotFound(new { mensaje = $"El usuario con ID {id} no fue encontrado" });
            }

            return Ok(usuario);
        }

        [HttpPost("crear")]
        public async Task<ActionResult<int>> PostUsuario([FromBody] CrearUsuarioRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email))
                {
                    return BadRequest(new { mensaje = "El email es obligatorio" });
                }

                if (string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { mensaje = "La contraseña es obligatoria" });
                }

                int nuevoId = await _usuarioRepository.CrearAsync(request);

                return CreatedAtAction(nameof(GetUsuario), new { id = nuevoId },
                                                           new { id = nuevoId, mensaje = "Usuario creado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrio un error al procesar ", detalle = ex.Message });
            }
        }

        [HttpPut("actualizar/{id}")]
        public async Task<ActionResult> PutUsuario(int id, [FromBody] EditarUsuarioRequest request)
        {
            try
            {
                var actualizado = await _usuarioRepository.ActualizarAsync(id, request);
                if (!actualizado)
                {
                    return NotFound(new { mensaje = $"El usuario con ID {id} no fue encontrado para actualizar" });
                }

                return Ok(new { mensaje = "Usuario actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al actualizar", detalle = ex.Message });
            }
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult> DeleteUsuario(int id)
        {
            try
            {
                var eliminado = await _usuarioRepository.EliminarAsync(id);
                if (!eliminado)
                {
                    return NotFound(new { mensaje = $"El usuario con ID {id} no fue encontrado para eliminar" });
                }

                return Ok(new { mensaje = "Usuario eliminado (desactivado) exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al eliminar", detalle = ex.Message });
            }
        }
    }
}