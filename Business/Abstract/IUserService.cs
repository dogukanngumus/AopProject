using Core.Entities;
using Core.Entities.Concrete;
using Entities.Concrete;

namespace Business.Abstract;

public interface IUserService
{
    List<OperationClaim> GetClaims(User user);
    void Add(User user);
    void UpdateRefreshToken(User user,string refreshToken, DateTime expiration);
    User GetByMail(string email);
    User GetByRefreshToken(string refreshToken);
}