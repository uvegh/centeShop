



namespace Identity.Application.Contracts;

public interface ITokenService
{
    Task<string> CreatAccessToken(ApiUser User);
    Task<RefreshToken> GetRefreshToken();
}
