namespace Arsenals.Domains.Guns;

/// <summary>
/// 装弾数
/// </summary>
public class Capacity
{
    private static readonly int MIN = 1;
    private static readonly int MAX = 5000;

    private readonly int _value;

    public Capacity(int value)
    {
        if (value < MIN) { throw new ArgumentOutOfRangeException(nameof(value), $"装弾数が最小値{MIN}より小さいです"); }
        if (value > MAX) { throw new ArgumentOutOfRangeException(nameof(value), $"装弾数が最大値{MAX}より大きいです"); }
        _value = value;
    }

    /// <summary>
    /// 装弾数
    /// </summary>
    public int Value => _value;

    public override string ToString() { return _value.ToString(); }
}
