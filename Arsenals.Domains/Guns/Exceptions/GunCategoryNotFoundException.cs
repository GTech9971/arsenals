namespace Arsenals.Domains.Guns.Exceptions;

/// <summary>
/// 銃のカテゴリーが存在しない例外
/// </summary>
[Serializable]
public class GunCategoryNotFoundException : Exception
{
    public GunCategoryNotFoundException(GunCategoryId id) : base($"カテゴリーID:{id}は存在しません") { }
    public GunCategoryNotFoundException(string message) : base(message) { }
    public GunCategoryNotFoundException(string message, Exception inner) : base(message, inner) { }
}
