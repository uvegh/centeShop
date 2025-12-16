
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using System.Text;


namespace Identity.Application.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _config;
    public TokenService( UserManager<ApiUser> userManager,IConfiguration config)
    {
        _config = config;
        _userManager = userManager;
    }
    public async Task<string> CreateAccessToken(ApiUser user){
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub,user.Id),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Name, user.FirstName)
        };
        var roles =await  _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role,r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwt_key"]!));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials:cred
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
        }

    public async Task<string> CreateRefreshToken(ApiUser user)
    {
        var currUser = _userManager.GetUserAsync(user);
    }
}
