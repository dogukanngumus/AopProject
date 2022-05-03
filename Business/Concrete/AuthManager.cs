using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;

namespace Business.Concrete;

public class AuthManager:IAuthService
{
    private IUserService _userService;
    private ITokenHelper _tokenHelper;

    public AuthManager(IUserService userService, ITokenHelper tokenHelper)
    {
        _userService = userService;
        _tokenHelper = tokenHelper;
    }

    public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
    {
        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash(password,out passwordHash,out passwordSalt);
        var user = new User
        {
            Email = userForRegisterDto.Email,
            FirstName = userForRegisterDto.FirstName,
            LastName = userForRegisterDto.LastName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Status = true
        };
        _userService.Add(user);
        return  new SuccessDataResult<User>(user,Messages.UserRegistered);
    }

    public IDataResult<User> Login(UserForLoginDto userForLoginDto)
    {
        var user = _userService.GetByMail(userForLoginDto.Email);
        if (user == null)
        {
            return new ErrorDataResult<User>(Messages.UserNotFound);
        }
        if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            return new ErrorDataResult<User>(Messages.PasswordError);
        }

        return new SuccessDataResult<User>(user, Messages.SuccessfulLogin);
    }

    public IDataResult<User> LoginWithRefreshToken(string refreshToken)
    {
        var user = _userService.GetByRefreshToken(refreshToken);
        if (user != null && user?.RefreshTokenExpiration > DateTime.Now)
        {
            return new SuccessDataResult<User>(user, Messages.SuccessfulLogin);
        }
        return new ErrorDataResult<User>(Messages.UserNotFound);
    }

    public IResult UserExists(string email)
    {
        if (_userService.GetByMail(email) != null)
        {
            return new ErrorResult(Messages.UserAlreadyExists);
        }
        return new SuccessResult();
    }

    public IDataResult<TokenDto> CreateToken(User user)
    {
        var claims = _userService.GetClaims(user);
        var token = _tokenHelper.CreateToken(user, claims);
        _userService.UpdateRefreshToken(user, token.RefreshToken, token.RefreshTokenExpiration);
        return new SuccessDataResult<TokenDto>(token, Messages.AccessTokenCreated);
    }
}