using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EliteSoftTask.Configurations;
using EliteSoftTask.Data.Database.Entities;
using EliteSoftTask.Services.Utils;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EliteSoftTask.Services;

public class TokenService
{
    private readonly JwtOptions _jwtOptions;

    public TokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public ServiceResponse<string> CreateToken(User user,string freeIpaId = "")
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(_jwtOptions.Issuer,audience: _jwtOptions.Issuer,
            claims: new []
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim("AuthSource",user.AuthSource.ToString()),
                new Claim(ClaimTypes.Sid,user.Id.ToString()),
                new Claim("IssuedAt",DateTime.UtcNow.ToString()),
                new Claim("FreeIPAId",freeIpaId.ToString())
            },
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return new ServiceResponse<string>(token);
    }
}