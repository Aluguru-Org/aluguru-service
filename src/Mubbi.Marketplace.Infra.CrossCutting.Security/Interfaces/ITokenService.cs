using Mubbi.Marketplace.Domain.Models;

namespace Mubbi.Marketplace.Infra.CrossCutting.Security.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
