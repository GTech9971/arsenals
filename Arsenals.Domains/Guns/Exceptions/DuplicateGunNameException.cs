using Arsenals.Domains.Exceptions;

namespace Arsenals.Domains.Guns.Exceptions;

/// <summary>
/// 銃の名称が被っている例外
/// </summary>
[Serializable]
public class DuplicateGunNameException : DuplicateException
{
    public DuplicateGunNameException(GunName gunName) : base($"銃の名称:{gunName}は既に存在します") { }

    public DuplicateGunNameException(string message) : base(message) { }
    public DuplicateGunNameException(string message, Exception inner) : base(message, inner) { }
}
