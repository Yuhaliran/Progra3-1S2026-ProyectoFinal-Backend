namespace ProyectoFinal.Progra3.Backend.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ProyectoFinal.Progra3.Backend.Modelos;
    using ProyectoFinal.Progra3.Backend.Modelos.Request.Bitacora;
    using ProyectoFinal.Progra3.Backend.Modelos.Response.Bitacora;
    using ProyectoFinal.Progra3.Backend.Repositorios.Interfaces;
    using System.Security.Claims;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ColaLecturaController : ControllerBase
    {
        private readonly IColaLecturaRepository _colaLecturaRepository;

        public ColaLecturaController(IColaLecturaRepository colaLecturaRepository)
        {
            _colaLecturaRepository = colaLecturaRepository;
        }

        private int ObtenerIdUsuario()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "IdUsuario") ?? 
                        User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier) ??
                        User.Claims.FirstOrDefault(c => c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
                        
            if (claim != null && int.TryParse(claim.Value, out int idUsuario))
            {
                return idUsuario;
            }
            throw new System.UnauthorizedAccessException("IdUsuario no encontrado en el token.");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColaLecturaResponse>>> ObtenerMiCola()
        {
            int idUsuario = ObtenerIdUsuario();
            var cola = await _colaLecturaRepository.ObtenerColaPorUsuarioAsync(idUsuario);
            return Ok(cola);
        }

        [HttpGet("{isbn}")]
        public async Task<ActionResult<ColaLecturaResponse>> ObtenerMiRegistro(string isbn)
        {
            int idUsuario = ObtenerIdUsuario();
            var registro = await _colaLecturaRepository.ObtenerRegistroColaAsync(idUsuario, isbn);
            if (registro == null)
            {
                return NotFound();
            }
            return Ok(registro);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarRegistro([FromBody] ColaLecturaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int idUsuario = ObtenerIdUsuario();
            await _colaLecturaRepository.AgregarOActualizarRegistroColaAsync(idUsuario, request);

            return Ok(new { Mensaje = "Registro guardado exitosamente." });
        }

        [HttpGet("estados")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<EstadoLectura>>> ObtenerEstados()
        {
            var estados = await _colaLecturaRepository.ObtenerEstadosLecturaAsync();
            return Ok(estados);
        }
    }
}
