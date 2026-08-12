using Microsoft.AspNetCore.Mvc;
using RestAPIFurb.Dtos;
using RestAPIFurb.Services;

namespace RestAPIFurb.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST api/auth/login -> 200 com token, ou 401 se credenciais inválidas
        // Usuário de teste (seed): login "admin", senha "123456"
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.AutenticarAsync(dto.Login, dto.Senha);
            if (token == null)
                return Unauthorized(new { erro = "Login ou senha inválidos" });

            return Ok(new { token });
        }
    }
}
