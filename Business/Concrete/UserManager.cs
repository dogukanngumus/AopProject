using Business.Abstract;
using Core.Entities;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete;

public class UserManager:IUserService
{
    IUserDal _userDal;

    public UserManager(IUserDal userDal)
    {
        _userDal = userDal;
    }

    public List<OperationClaim> GetClaims(User user)
    {
        return _userDal.GetClaims(user);
    }

    public void Add(User user)
    {
        _userDal.Add(user);
    }
    
    public void UpdateRefreshToken(User user, string refreshToken, DateTime expiration)
    {
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiration = expiration;
        _userDal.Update(user);
    }
    public User GetByMail(string email)
    {
        return _userDal.Get(u => u.Email == email);
    }

    public User GetByRefreshToken(string refreshToken)
    {
        return _userDal.Get(u => u.RefreshToken == refreshToken);
    }
}