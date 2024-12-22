namespace Arsenals.Domains.Bullets;

/// <summary>
/// 弾丸のダメージ
/// </summary>
public record class Damage
{
    private static readonly int MIN = 1;
    private static readonly int MAX = 50;

    /// <summary>
    /// 弾丸のダメージ
    /// </summary>
    public int Value { get; init; }

    public Damage(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, MIN, nameof(value));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, MAX, nameof(value));
        Value = value;
    }

}
