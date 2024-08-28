namespace Arsenals.Domains.Users;

public interface IUserRepository
{
    Task<User?> FetchAsync(UserId id);

    Task<bool> CheckPasswordAsync(UserId id, Password password);
}
