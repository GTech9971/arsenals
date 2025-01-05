using Arsenals.Domains.Guns;

namespace Microsoft.Extensions.Configuration;

public static class IConfigurationExtension
{
    /// <summary>
    /// 銃画像のダウンロードURL
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static string? GetGunImageDownloadUrl(this IConfiguration configuration)
    {
        return configuration[GunImage.DOWNLOAD_KEY];
    }

    /// <summary>
    /// 銃画像の保存先パスのルート
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static string? GetGunImageRoot(this IConfiguration configuration)
    {
        return configuration[GunImage.ROOT_KEY];
    }
}
