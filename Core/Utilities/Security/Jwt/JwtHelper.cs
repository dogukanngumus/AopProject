using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.Utilities.Security.Jwt;

public class JwtHelper:ITokenHelper
{
    public IConfiguration _configuration;
    public TokenOptions _tokenOptions;
    public DateTime _accessTokenExpiration;
    public DateTime _refreshTokenExpiration;
    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
        _tokenOptions = _configuration.GetSection("TokenOptions").Get<TokenOptions>();
        _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
        _refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpiration);
    }

    public TokenDto CreateToken(User user, List<OperationClaim> operationClaims)
    {
        var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
        var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
        var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var token = jwtSecurityTokenHandler.WriteToken(jwt);

        return new TokenDto()
        {
            AccessToken = token,
            AccessTokenExpiration = _accessTokenExpiration,
            RefreshToken = CreateRefreshToken(),
            RefreshTokenExpiration = _refreshTokenExpiration
        };
    }
    
    public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user, 
        SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
    {
        var jwt = new JwtSecurityToken(
            issuer:tokenOptions.Issuer,
            audience:tokenOptions.Audience,
            expires:_accessTokenExpiration,
            notBefore:DateTime.Now,
            claims: SetClaims(user,operationClaims),
            signingCredentials:signingCredentials
        );
        return jwt;
    }

    private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
    {
        var claims = new List<Claim>();
        claims.AddNameIdentifier(user.Id.ToString());
        claims.AddEmail(user.Email);
        claims.AddName($"{user.FirstName} {user.LastName}");
        claims.AddRoles(operationClaims.Select(c=>c.Name).ToArray());
        return claims;
    }
    
    private string CreateRefreshToken()
    {
        var numberByte = new Byte[32];
        using var rand = RandomNumberGenerator.Create();
        rand.GetBytes(numberByte);
        return Convert.ToBase64String(numberByte);
    }
}