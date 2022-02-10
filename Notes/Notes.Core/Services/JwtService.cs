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
        private readonly AuthSettings authSettings;

        public JwtService(IOptions<AuthSettings> authSettings)
        {
            this.authSettings = authSettings.Value;
        }

        public string Generate(string email)
        {
            byte[] key = Encoding.ASCII.GetBytes(authSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddHours(authSettings.LifeTimeHours),
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
