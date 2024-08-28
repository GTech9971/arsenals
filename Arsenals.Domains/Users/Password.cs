using System;

namespace Arsenals.Domains.Users;

public class Password
{
    private readonly string _value;

    public Password(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));

        _value = value;
    }

    public string Value => _value;
}
