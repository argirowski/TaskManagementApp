namespace Application.Interfaces
{
    using Domain.Entities;

    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
