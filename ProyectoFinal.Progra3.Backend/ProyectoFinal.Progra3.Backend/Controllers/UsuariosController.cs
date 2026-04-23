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

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponse>> GetUsuario(int id)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);

            if (usuario == null)
            {
                return NotFound(new { mensaje = $"El usuario con ID {id} no fue encontrado" });
            }

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<int>> PostUsuario([FromBody] CrearUsuarioRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email))
                {
                    return BadRequest(new { mensaje = "El email es obligatorio" });
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
    }
}