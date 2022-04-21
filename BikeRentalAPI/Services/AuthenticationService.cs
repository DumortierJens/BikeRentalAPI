namespace BikeRentalAPI.Services;

public record UserInfo(string Username);
public record AuthenticationRequestBody(string Username, string Password);

public interface IAuthenticationService
{
    UserInfo ValidateUser(string username, string password);
}

public class AuthenticationService : IAuthenticationService
{
    public UserInfo ValidateUser(string username, string password)
    {
        return new UserInfo(username);
    }
}


