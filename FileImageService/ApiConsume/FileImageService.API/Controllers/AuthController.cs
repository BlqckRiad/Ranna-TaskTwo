using FileImageService.DtoLayer.Dtos.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FileImageService.API.Controllers
{
    /// <summary>
    /// Kimlik doğrulama işlemleri için API endpoint'leri
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// sa
        /// Kullanıcı girişi yapar ve JWT token döner
        /// </summary>
        /// <param name="loginDto">Kullanıcı giriş bilgileri</param>
        /// <returns>JWT token ve son kullanma tarihi</returns>
        /// <response code="200">Başarılı giriş</response>
        /// <response code="401">Geçersiz kullanıcı bilgileri</response>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // Burada normalde kullanıcı doğrulaması yapılır
            // Örnek olarak basit bir kontrol yapıyoruz
            if (loginDto.Username == "admin" && loginDto.Password == "123456")
            {
                var token = GenerateJwtToken(loginDto.Username, "Admin");
                return Ok(token);
            }
            else if (loginDto.Username == "user" && loginDto.Password == "123456")
            {
                var token = GenerateJwtToken(loginDto.Username, "User");
                return Ok(token);
            }

            return Unauthorized();
        }

        private TokenDto GenerateJwtToken(string username, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.Now.AddHours(1);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new TokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
} 