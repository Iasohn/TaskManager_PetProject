using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaskManagerPet.Models;


namespace TaskManagerPet.Services
{
    public class TokenService
    {
        private IConfiguration _conf;

        private SymmetricSecurityKey _Key;
        public TokenService(IConfiguration conf)
        {
            _conf = conf;
            _Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["JWT:SigningKey"]));
        }

        public string CreateToken(User user,string role)
        {
            var claims = new Claim[]
            {
   
                new Claim(ClaimTypes.Email, user.Email),           
                new Claim(ClaimTypes.Role, role)
            };

            var creds = new SigningCredentials(_Key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _conf["JWT:Issuer"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(150),
                Audience = _conf["JWT:Audience"],
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var Token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(Token);            
        }

        public string RefreshToken()
        {
            var randomNember = new byte[64];

            using (var NumberGenerator = RandomNumberGenerator.Create())
            {
                NumberGenerator.GetBytes(randomNember);
            }
            return Convert.ToBase64String(randomNember);
        }

    }
}
