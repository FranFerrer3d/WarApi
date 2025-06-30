using WarApi.Models;

namespace WarApi.Services.Security
{
    public interface ITokenService
    {
        string CreateToken(Player player);
    }
}
