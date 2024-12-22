namespace Arsenals.Domains.Bullets;

/// <summary>
/// 弾丸名称
/// </summary>
public record class BulletName
{
    private static readonly int MIN = 1;
    private static readonly int MAX = 50;

    public string Value { get; init; }

    public BulletName(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, MIN, nameof(value));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, MAX, nameof(value));
        Value = value;
    }
}
