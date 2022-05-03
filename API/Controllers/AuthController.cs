using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController:Controller
{
    private IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService; 
    }

    [HttpPost("login")]
    public ActionResult Login(UserForLoginDto userForLoginDto)
    {
        var userForLogin = _authService.Login(userForLoginDto);
        if (!userForLogin.Success)
        {
            return BadRequest(userForLogin.Message);
        }

       var token = _authService.CreateToken(userForLogin.Data);
       if (token.Success)
       {
           return Ok(token.Data);
       }

       return BadRequest(token.Message);
    }
    
    [HttpPost("refreshTokenLogin")]
    public ActionResult Login(string refreshToken)
    {
        var userForLogin = _authService.LoginWithRefreshToken(refreshToken);
        if (!userForLogin.Success)
        {
            return BadRequest(userForLogin.Message);
        }

        var token = _authService.CreateToken(userForLogin.Data);
        if (token.Success)
        {
            return Ok(token.Data);
        }

        return BadRequest(token.Message);
    }
    
    [HttpPost("register")]
    public ActionResult Register(UserForRegisterDto userForRegisterDto)
    {
        var userExists = _authService.UserExists(userForRegisterDto.Email);
        if (!userExists.Success)
        {
            return BadRequest(userExists.Message);
        }

        var registerUser = _authService.Register(userForRegisterDto,userForRegisterDto.Password);
        var token = _authService.CreateToken(registerUser.Data);
        if (token.Success)
        {
            return Ok(token.Data);
        }

        return BadRequest(token.Message);
    }
}