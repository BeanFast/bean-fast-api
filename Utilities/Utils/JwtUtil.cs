using BusinessObjects.Enums;
using BusinessObjects.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.Utils
{
    public static class JwtUtil
    {
        public static string GenerateToken(User user)
        {
            //const string secretKey = configuration[]
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey secrectKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTContants.JWTSECRET));
            var credentials = new SigningCredentials(secrectKey, SecurityAlgorithms.HmacSha256Signature);
            List<Claim> claims = new List<Claim>()
            {
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, "thanh"),
                new Claim(ClaimTypes.Role, RoleName.ADMIN.ToString()),
            };
            //if (guidClaim != null) claims.Add(new Claim(guidClaim.Item1, guidClaim.Item2.ToString()));
            var expires = DateTime.Now.AddDays(1);
            var token = new JwtSecurityToken(expires: expires, claims: claims, signingCredentials: credentials);
            return jwtHandler.WriteToken(token);
        }

        public static bool VerifyToken(string token) 
        {
            return false;
        }
    }
}
