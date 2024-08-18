namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃ID
/// </summary>
public class GunId : IEquatable<GunId>
{
    private static readonly int MIN = 100;

    private readonly int _value;

    public GunId(int value)
    {
        if (value < MIN)
        {
            throw new ArgumentException($"銃IDの値が最小値{MIN}より小さいです");
        }

        _value = value;
    }

    /// <summary>
    /// 銃ID
    /// </summary>
    public int Value => _value;

    /// <summary>
    /// 銃の画像のURLを取得する
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public Uri ImageUrl(string root)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(root, nameof(root));
        return new Uri(Path.Combine(root, _value.ToString()));
    }

    /// <summary>
    /// 銃の画像の保存先を取得する
    /// </summary>
    /// <param name="root"></param>
    /// <returns></returns>
    public string ImagePath(string root)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(root, nameof(root));
        return Path.Combine(root, _value.ToString());
    }

    public bool Equals(GunId? other)
    {
        if (object.ReferenceEquals(null, other)) { return false; }
        if (object.ReferenceEquals(this, other)) { return true; }
        return _value == other.Value;
    }

    public override bool Equals(object? obj) { return Equals(obj as GunId); }
    public override int GetHashCode() { return _value.GetHashCode(); }

    public static bool operator ==(GunId left, GunId right)
    {
        if (object.ReferenceEquals(left, right)) { return true; }
        return left.Equals(right);
    }

    public static bool operator !=(GunId left, GunId right) => !(left == right);

    public override string ToString() { return _value.ToString(); }
}
