using Microsoft.Extensions.Configuration;

namespace Arsenals.Domains.Guns;

/// <summary>
/// 銃画像
/// </summary>
public record class GunImage
{
    public static readonly string DOWNLOAD_KEY = "Images:downloadRoot";
    public static readonly string ROOT_KEY = "Images:root";

    private readonly string _root;
    private readonly string _downloadRoot;

    /// <summary>
    /// 銃画像ID
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// 画像拡張子
    /// </summary>
    public string Extension { get; init; }

    public GunImage(int id, string extension, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        Id = id;

        ArgumentNullException.ThrowIfNull(extension, nameof(extension));
        Extension = extension;

        string? downloadRoot = configuration[DOWNLOAD_KEY];
        ArgumentNullException.ThrowIfNullOrWhiteSpace(downloadRoot, nameof(downloadRoot));
        _downloadRoot = downloadRoot;

        string? root = configuration[ROOT_KEY];
        ArgumentNullException.ThrowIfNullOrWhiteSpace(root, nameof(root));
        _root = root;
    }

    /// <summary>
    /// ダウンロードURLパス
    /// https://arsenals/assets/guns/images/G-0001/{id}.拡張子
    /// </summary>
    /// <param name="gunId"></param>
    /// <returns></returns>
    public Uri DownloadUrl(GunId gunId)
    {
        ArgumentNullException.ThrowIfNull(gunId, nameof(gunId));
        return new Uri($"{_downloadRoot}/{gunId.Value}/{Id}{Extension}");
    }

    /// <summary>
    /// /root/arsenals/guns/images/{gunId}/{id}.拡張子
    /// </summary>
    /// <param name="gunId"></param>
    /// <returns></returns>
    public string Path(GunId gunId)
    {
        ArgumentNullException.ThrowIfNull(gunId, nameof(gunId));
        return System.IO.Path.Combine(_root, gunId.Value, $"{Id}{Extension}");
    }
}
