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

                new Claim(ClaimTypes.Name, user.FullName ?? user.Code),
                new Claim(ClaimTypes.Role, user.Role!.EnglishName),
            };
            //if (guidClaim != null) claims.Add(new Claim(guidClaim.Item1, guidClaim.Item2.ToString()));
            var expires = TimeUtil.GetCurrentVietNamTime().AddDays(JWTConstants.JwtExpiresInMinutes);
            var token = new JwtSecurityToken(expires: expires, claims: claims, signingCredentials: credentials);
            return jwtHandler.WriteToken(token);
        }
        public static string GenerateRefreshToken(User user)
        {
            //const string secretKey = configuration[]
            Console.Write(user.Id);
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey secrectKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTConstants.JwtRefreshTokenSecret));
            var credentials = new SigningCredentials(secrectKey, SecurityAlgorithms.HmacSha256Signature);
            List<Claim> claims = new List<Claim>
            {
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            };
            //if (guidClaim != null) claims.Add(new Claim(guidClaim.Item1, guidClaim.Item2.ToString()));
            var expires = TimeUtil.GetCurrentVietNamTime().AddDays(JWTConstants.JwtRefreshTokenDays);
            var token = new JwtSecurityToken(expires: expires, claims: claims, signingCredentials: credentials);
            return jwtHandler.WriteToken(token);
        }
        public static bool ValidateRefreshToken(string token)
        {
            //const string secretKey = configuration[]
            var tokenHandler = new JwtSecurityTokenHandler();
            
            SymmetricSecurityKey secrectKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTConstants.JwtRefreshTokenSecret));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secrectKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                return validatedToken != null;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as per your requirements
                Console.WriteLine($"Exception occurred while validating token: {ex.Message}");
                return false;
            }
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