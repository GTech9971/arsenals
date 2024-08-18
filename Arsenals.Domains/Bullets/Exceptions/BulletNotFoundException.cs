using Arsenals.Domains.Exceptions;

namespace Arsenals.Domains.Bullets.Exceptions;

/// <summary>
/// 弾丸が存在しない例外
/// </summary>
[Serializable]
public class BulletNotFoundException : NotFoundException
{
    public BulletNotFoundException(BulletId bulletId) : base($"弾丸ID:{bulletId}は存在しません") { }
    public BulletNotFoundException(string message) : base(message) { }
    public BulletNotFoundException(string message, Exception inner) : base(message, inner) { }
}
