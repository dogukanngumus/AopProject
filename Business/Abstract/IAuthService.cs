using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using Microsoft.Extensions.Logging;

namespace Business.Abstract;

public interface IAuthService
{
    IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password);
    IDataResult<User> Login(UserForLoginDto userForLoginDto);
    IDataResult<User> LoginWithRefreshToken(string refreshToken);
    IResult UserExists(string email);
    IDataResult<TokenDto> CreateToken(User user);
}