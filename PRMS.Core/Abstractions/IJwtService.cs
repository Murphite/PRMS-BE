using PRMS.Domain.Entities;

namespace PRMS.Core.Abstractions;

public interface IJwtService
{
    public string GenerateToken(User user, IList<string> roles);
}