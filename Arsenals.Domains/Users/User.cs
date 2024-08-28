using System.Collections.Immutable;

namespace Arsenals.Domains.Users;

public class User
{
    private readonly UserId _id;
    private readonly IEnumerable<UserRole> _roles;

    public User(UserId id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        _id = id;
        _roles = [];
    }

    public User(UserId id, IEnumerable<UserRole> roles)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        ArgumentNullException.ThrowIfNull(roles, nameof(roles));
        _id = id;
        _roles = roles.Distinct();
    }

    public UserId UserId => _id;

    public ImmutableList<UserRole> Roles => _roles.ToImmutableList();
}
