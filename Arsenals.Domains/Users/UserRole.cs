using System;

namespace Arsenals.Domains.Users;

public class UserRole
{
    private readonly string _value;

    public UserRole(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        _value = value;
    }

    public string Value => _value;
}
