namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃の名称
/// </summary>
public record GunName
{
    private static readonly int MIN_LEN = 1;

    public string Value { get; init; }

    public GunName(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, MIN_LEN, nameof(value));

        Value = value;
    }
}
