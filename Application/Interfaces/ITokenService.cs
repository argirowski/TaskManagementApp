using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
        string GenerateRefreshToken();
        Task<string> GenerateAndSaveRefreshTokenAsync(User user);
        Task<TokenResponseDTO> CreateTokenResponseAsync(User user);
    }
}
