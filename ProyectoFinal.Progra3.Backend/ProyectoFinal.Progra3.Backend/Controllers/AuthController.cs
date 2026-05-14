namespace ProyectoFinal.Progra3.Backend.Controllers
{
    using System.Text;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using ProyectoFinal.Progra3.Backend.Modelos.Request.Auth;
    using ProyectoFinal.Progra3.Backend.Modelos.Response.Auth;
    using ProyectoFinal.Progra3.Backend.Repositorios.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var usuario = await _usuarioRepository.ObtenerPorEmailParaAuthAsync(request.Email);

            if (usuario == null)
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas" });
            }

            bool passwordValido = BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash);

            if (!passwordValido)
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas" });
            }

            var token = GenerarJwtToken(usuario);

            return Ok(new LoginResponse { Token = token, Mensaje = "Login exitoso" });
        }

        private string GenerarJwtToken(UsuarioAuthModel usuario)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("SecretKey no está configurada");
            }

            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("Rol", usuario.Rol),
                new Claim("IdRol", usuario.IdRol.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: null, 
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2), 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
