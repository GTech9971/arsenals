using Arsenals.Domains.Exceptions;

namespace Arsenals.Domains.Guns.Exceptions;

/// <summary>
/// 銃のカテゴリー名が重複した例外
/// </summary>
public class DuplicateGunCategoryNameException : DuplicateException
{
    public DuplicateGunCategoryNameException() : base() { }
    public DuplicateGunCategoryNameException(GunCategoryName gunCategoryName) : base($"カテゴリー名:{gunCategoryName}は既に存在します") { }
    public DuplicateGunCategoryNameException(string message, Exception inner) : base(message, inner) { }
}
