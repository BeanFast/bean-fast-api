using BusinessObjects.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Utilities.Constants;
using Utilities.Enums;

namespace Utilities.Utils
{
    public static class JwtUtil
    {
        public static string GenerateToken(User user)
        {
            //const string secretKey = configuration[]
            Console.Write(user.Id);
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey secrectKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTConstants.JwtSecret));
            var credentials = new SigningCredentials(secrectKey, SecurityAlgorithms.HmacSha256Signature);
            List<Claim> claims = new List<Claim>
            {
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.Id.ToString()),

                new Claim(ClaimTypes.Name, user.FullName!),
                new Claim(ClaimTypes.Role, user.Role!.Name),
            };
            //if (guidClaim != null) claims.Add(new Claim(guidClaim.Item1, guidClaim.Item2.ToString()));
            var expires = DateTime.Now.AddDays(JWTConstants.JwtExpiresInMinutes);
            var token = new JwtSecurityToken(expires: expires, claims: claims, signingCredentials: credentials);
            return jwtHandler.WriteToken(token);
        }
        public static string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }
    }
}