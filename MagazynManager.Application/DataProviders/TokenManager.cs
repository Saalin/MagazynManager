﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MagazynManager.Application.DataProviders
{
    public class TokenManager
    {
        private readonly string _key;

        public TokenManager(string key)
        {
            _key = key;
        }

        public TokenValidationResult ValidateToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(_key);
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            if (claims == null)
            {
                return new TokenValidationResult { IsValid = false };
            }
            var parseResult = Guid.TryParse(claims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out var uzytkownikId);
            Guid.TryParse(claims.Claims.FirstOrDefault(c => c.Type.Contains("PrzedsiebiorstwoId"))?.Value, out var przedsiebiorstwoId);
            if (parseResult)
            {
                return new TokenValidationResult
                {
                    IsValid = true,
                    Identity = new IdentificationAggregate
                    {
                        UserId = uzytkownikId,
                        PrzedsiebiorstwoId = przedsiebiorstwoId
                    }
                };
            }

            return new TokenValidationResult { IsValid = false };
        }

        public JwtSecurityToken CreateJWTToken(IEnumerable<Claim> claims)
        {
            var key = Encoding.UTF8.GetBytes(_key);
            return new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
             );
        }
    }
}