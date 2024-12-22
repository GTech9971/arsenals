using System.Data;

namespace Arsenals.Domains.Bullets.Exceptions;

public class DuplicateBulletNameException : DuplicateNameException
{
    public DuplicateBulletNameException() : base() { }

    public DuplicateBulletNameException(BulletName name) : base($"弾丸名:{name}は既に存在します") { }
    public DuplicateBulletNameException(string message, Exception inner) : base(message, inner) { }
}
