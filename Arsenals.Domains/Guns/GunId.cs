using System.Text.RegularExpressions;

namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃ID
/// </summary>
/// <example>G-1201</example>
public record GunId
{
    private const string PATTERN = @"^G-\d{4}$";

    public string Value { get; init; }

    /// <summary>
    /// IDのインデックス
    /// </summary>
    public int Index => Convert.ToInt32(Value.Replace("G-", ""));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="FormatException"></exception>
    public GunId(string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        if (Regex.IsMatch(value, PATTERN) == false) { throw new FormatException("銃IDの形式が不正です。"); }
        Value = value;
    }


    /// <summary>
    /// 銃の画像のURLを取得する
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public Uri ImageUrl(string root)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(root, nameof(root));
        return new Uri(Path.Combine(root, Value.ToString()));
    }

    /// <summary>
    /// 銃の画像の保存先を取得する
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public string ImagePath(string root)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(root, nameof(root));
        return Path.Combine(root, Value.ToString());
    }
}
