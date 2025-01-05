using System.Text.RegularExpressions;

namespace Arsenals.Domains.Bullets;

/// <summary>
/// 弾丸ID
/// </summary>
public record BulletId
{
    private const string PATTERN = @"^B-\d{4}$";
    // private static readonly int MIN = 100;

    public string Value { get; init; }

    public BulletId(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        if (Regex.IsMatch(value, PATTERN) == false) { throw new FormatException("弾丸IDの形式が不正です。"); }
        Value = value;
    }

    public BulletId Next()
    {
        return new BulletId($"B-{(Index + 1).ToString().PadLeft(4, '0')}");
    }

    /// <summary>
    /// IDのインデックス
    /// </summary>
    public int Index => Convert.ToInt32(Value.Replace("B-", ""));
}
