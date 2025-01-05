namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃画像
/// </summary>
public record class GunImage
{
    /// <summary>
    /// ダウンロードURL
    /// </summary>
    public static readonly string DOWNLOAD_KEY = "Images:downloadRoot";
    public static readonly string ROOT_KEY = "Images:root";

    /// <summary>
    /// 銃画像ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// 画像拡張子
    /// </summary>
    public string Extension { get; init; }

    public GunImage(int id, string extension)
    {
        ArgumentNullException.ThrowIfNull(extension, nameof(extension));

        Id = id;
        Extension = extension;
    }

    /// <summary>
    /// ダウンロードURLパス
    /// https://arsenals/assets/guns/images/G-0001/{id}.拡張子
    /// </summary>
    /// <param name="gunId"></param>
    /// <returns></returns>
    public Uri DownloadUrl(GunId gunId, string downloadRoot)
    {
        ArgumentNullException.ThrowIfNull(gunId, nameof(gunId));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(downloadRoot, nameof(downloadRoot));

        return new Uri($"{downloadRoot}/{gunId.Value}/{Id}{Extension}");
    }

    /// <summary>
    /// /root/arsenals/guns/images/{gunId}/{id}.拡張子
    /// </summary>
    /// <param name="gunId"></param>
    /// <returns></returns>
    public string Path(GunId gunId, string root)
    {
        ArgumentNullException.ThrowIfNull(gunId, nameof(gunId));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(root, nameof(root));

        return System.IO.Path.Combine(root, gunId.Value, $"{Id}{Extension}");
    }
}
