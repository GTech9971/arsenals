namespace Arsenals.Domains.Guns;

/// <summary>
/// 装弾数
/// </summary>
public record Capacity
{
    public const int MIN = 1;
    public const int MAX = 5000;

    public int Value { get; init; }

    public Capacity(int value)
    {
        if (value < MIN) { throw new ArgumentOutOfRangeException(nameof(value), $"装弾数が最小値{MIN}より小さいです"); }
        if (value > MAX) { throw new ArgumentOutOfRangeException(nameof(value), $"装弾数が最大値{MAX}より大きいです"); }
        Value = value;
    }

}
