using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RestAPIFurb.Dao;

namespace RestAPIFurb.Services
{
    public interface IAuthService
    {
        Task<string?> AutenticarAsync(string login, string senha);
    }

    public class AuthService : IAuthService
    {
        private readonly IUsuarioDao _usuarioDao;
        private readonly IConfiguration _config;

        public AuthService(IUsuarioDao usuarioDao, IConfiguration config)
        {
            _usuarioDao = usuarioDao;
            _config = config;
        }

        public async Task<string?> AutenticarAsync(string login, string senha)
        {
            var usuario = await _usuarioDao.ObterPorLoginAsync(login);
            if (usuario == null) return null;

            var senhaHash = CalcularHash(senha);
            if (senhaHash != usuario.SenhaHash) return null;

            return GerarToken(usuario.Login);
        }

        private static string CalcularHash(string texto)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(texto));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        private string GerarToken(string login)
        {
            var chave = _config["Jwt:Key"]!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave));
            var credenciais = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
