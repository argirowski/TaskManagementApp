using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration, IUserRepository userRepository, ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _logger = logger;
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.UserEmail)
            };

            var jwtKey = _configuration.GetValue<string>("JWT:Key");
            if (string.IsNullOrEmpty(jwtKey))
            {
                _logger.LogError("JWT key is missing or empty in configuration for user {UserId}", user.Id);
                throw new BadRequestException("JWT key is not configured. Please contact system administrator.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var expirationMinutes = _configuration.GetValue<int>("JWT:ExpirationInMinutes");
            var issuer = _configuration.GetValue<string>("JWT:Issuer");
            var audience = _configuration.GetValue<string>("JWT:Audience");

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            _logger.LogInformation("Access token generated successfully for user {UserId} (Email: {UserEmail}) - Expires in {ExpirationMinutes} minutes", 
                user.Id, user.UserEmail, expirationMinutes);
            return jwt;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            _logger.LogInformation("Generating and saving refresh token for user {UserId} (Email: {UserEmail})", user.Id, user.UserEmail);

            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateUserAsync(user);

            _logger.LogInformation("Refresh token generated and saved successfully for user {UserId} (Email: {UserEmail}) - Expires in 7 days", 
                user.Id, user.UserEmail);
            return refreshToken;
        }

        public async Task<TokenResponseDTO> CreateTokenResponseAsync(User user)
        {
            _logger.LogInformation("Creating token response for user {UserId} (Email: {UserEmail}, Username: {UserName})", 
                user.Id, user.UserEmail, user.UserName);

            var accessToken = CreateToken(user);
            var refreshToken = await GenerateAndSaveRefreshTokenAsync(user);

            var response = new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserName = user.UserName
            };

            _logger.LogInformation("Token response created successfully for user {UserId} (Email: {UserEmail})", user.Id, user.UserEmail);
            return response;
        }
    }
}
