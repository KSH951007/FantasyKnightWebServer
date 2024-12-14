using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace FantasyKnightWebServer.Security
{
    public class JwtTokenService
    {
        private readonly string _secretKey;
        public JwtTokenService(string secretKey)
        {
            _secretKey = secretKey;
        }

        public string GenerateJwtToken(string uuid)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                new System.Security.Claims.Claim("sub", uuid)  // 'sub'는 사용자 ID 또는 UUID
            }),
                Expires = DateTime.UtcNow.AddDays(30),  // 토큰 만료 시간
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);  // 생성된 JWT 토큰 반환
        }
        public string ExtractUuidFromJwt(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            try
            {
                // JWT 토큰을 검증하고 파싱
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                // 토큰 검증 및 파싱
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;

                // 'sub' 클레임에서 UUID 추출
                string userId = jwtToken.Subject;

                return userId;
            }
            catch (Exception)
            {
                // 토큰 검증에 실패한 경우 예외 처리
                throw new UnauthorizedAccessException("Invalid JWT Token");
            }
        }

    }
}
