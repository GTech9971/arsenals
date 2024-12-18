namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃のカテゴリー名
/// </summary>
public record GunCategoryName
{
    private static readonly int MAX = 50;

    public string Value { get; init; }

    public GunCategoryName(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, MAX, nameof(value));

        Value = value;
    }

}
