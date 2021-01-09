using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KeePassShtokal.Infrastructure.Entities;
using Microsoft.IdentityModel.Tokens;

namespace KeePassShtokal.AppCore.Helpers
{
    public static class TokenHelper
    {
        public static string GetToken(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mega secret key (Not!)"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.GivenName, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            };

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:44323",
                audience: "https://localhost:44323",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signinCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
    }
}
