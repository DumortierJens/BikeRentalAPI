namespace BikeRentalAPI.Services;

public interface ITokenService
{
    string GenerateToken(UserInfo user);
}

public class JWTTokenService : ITokenService
{
    private readonly AuthenticationSettings _settings;

    public JWTTokenService(IOptions<AuthenticationSettings> authSettings)
    {
        _settings = authSettings.Value;
    }

    public string GenerateToken(UserInfo user)
    {
        // 1. Symmetric Key maken op basis waarde uit settings file
        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_settings.SecretForKey));

        // 2. Credentials om de token te signen
        var signingCredentails = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // 3. Claims aanmaken
        var claimsForToken = new List<Claim>();
        claimsForToken.Add(new Claim("given_name", user.Username));

        // 4. Token maken
        var jwtSecurityToken = new JwtSecurityToken(
            _settings.Issuer, //Issuer uit appsettings
            _settings.Audience, //Audience uit appsettings
            claimsForToken, //claims
            DateTime.UtcNow, //start token valid
            DateTime.UtcNow.AddHours(1), //hoelang token valid
            signingCredentails //signing key
        );
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        // 5. Return token
        return token;
    }
}

