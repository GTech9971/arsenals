using System;

namespace Arsenals.Domains.Users;

public class UserId
{

    private readonly string _value;

    public UserId(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        _value = value;
    }

    public string Value => _value;
}
