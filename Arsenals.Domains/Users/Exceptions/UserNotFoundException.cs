using Arsenals.Domains.Exceptions;

namespace Arsenals.Domains.Users.Exceptions;

/// <summary>
/// ユーザーが存在しない例外
/// </summary>
[Serializable]
public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(UserId userId) : base($"ユーザー:{userId}が存在しません") { }

    public UserNotFoundException(string message) : base(message) { }

    public UserNotFoundException(string message, Exception inner) : base(message, inner) { }
}
