using System.Text.RegularExpressions;

namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃のカテゴリーID
/// </summary>
/// <example>C-9801</example>
public record class GunCategoryId
{
    private const string PATTERN = @"^C-\d{4}$";
    public string Value { get; init; }

    /// <summary>
    /// IDのインデックス
    /// </summary>
    public int Index => Convert.ToInt32(Value.Replace("C-", ""));

    public GunCategoryId(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        if (Regex.IsMatch(value, PATTERN) == false) { throw new FormatException("銃カテゴリーIDの形式が不正です。"); }
        Value = value;
    }

}
