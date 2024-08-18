using Arsenals.Domains.Exceptions;

namespace Arsenals.Domains.Guns.Exceptions;

/// <summary>
/// 銃が存在しない例外
/// </summary>
[Serializable]
public class GunNotFoundException : NotFoundException
{
    public GunNotFoundException(GunId id) : base($"銃ID:{id}は存在しません") { }
    public GunNotFoundException(string message) : base(message) { }
    public GunNotFoundException(string message, Exception inner) : base(message, inner) { }
}
