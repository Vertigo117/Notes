using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Notes.Core.Contracts;
using Notes.Core.Interfaces;
using Notes.Core.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Notes.Core.Services
{
    /// <summary>
    /// Сервис генерации Java Web Token'ов
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly IOptions<AuthSettings> authSettings;

        private AuthSettings AuthSettings 
        { 
            get
            {
                return authSettings.Value;
            }
        }

        public JwtService(IOptions<AuthSettings> authSettings)
        {
            this.authSettings = authSettings;
        }

        public string Generate(string email, string name)
        {
            byte[] key = Encoding.ASCII.GetBytes(AuthSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, name),
                }),
                Expires = DateTime.UtcNow.AddHours(AuthSettings.LifeTimeHours),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
