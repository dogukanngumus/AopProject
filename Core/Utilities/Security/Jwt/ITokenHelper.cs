using Core.Entities;
using Core.Entities.Concrete;

namespace Core.Utilities.Security.Jwt;

public interface ITokenHelper
{
    TokenDto CreateToken(User user, List<OperationClaim> operationClaims);
}