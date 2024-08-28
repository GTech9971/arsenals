using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Arsenals.Domains.Users;
using Arsenals.Domains.Users.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace Arsenals.WebApi;

public class JwtHandler
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public JwtHandler(IConfiguration configuration,
                        IUserRepository userRepository)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));

        _configuration = configuration;
        _userRepository = userRepository;
    }

    public async Task<JwtSecurityToken> GetTokenAsync(string userIdVal)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(userIdVal, nameof(userIdVal));
        UserId userId = new UserId(userIdVal);
        User? user = await _userRepository.FetchAsync(userId);
        if (user == null) { throw new UserNotFoundException(userId); }

        JwtSecurityToken jwt = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: GetClaims(user),
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpirationTimeInMinutes"])),
            signingCredentials: GetSigningCredentials()
        );
        return jwt;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(
            _configuration["JwtSettings:SecurityKey"]!
        );
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private List<Claim> GetClaims(User user)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.Value)
        };

        user.Roles.ForEach(x =>
        {
            claims.Add(new Claim(ClaimTypes.Role, x.Value));
        });

        return claims;
    }
}
