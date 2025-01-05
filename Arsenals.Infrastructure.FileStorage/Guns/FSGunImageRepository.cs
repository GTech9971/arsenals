using Arsenals.Domains.Guns;
using Arsenals.Infrastructure.Ef;
using Arsenals.Infrastructure.Ef.Guns;
using Microsoft.Extensions.Configuration;

namespace Arsenals.Infrastructure.FileStorage.Guns;

public class FSGunImageRepository : IGunImageRepository
{

    private readonly IConfiguration _configuration;
    private readonly string _root;

    private readonly ArsenalDbContext _context;

    public FSGunImageRepository(IConfiguration configuration,
                                    ArsenalDbContext context)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        _configuration = configuration;
        _context = context;

        string? val = _configuration[GunImage.ROOT_KEY];
        ArgumentNullException.ThrowIfNullOrWhiteSpace(val, nameof(_configuration));
        _root = val;
    }

    public Task DeleteAsync(GunImage gunImage)
    {
        throw new NotImplementedException();
        // return Task.Run(() =>
        // {
        //     if (File.Exists(id.ImagePath(_root)))
        //     {
        //         File.Delete(id.ImagePath(_root));
        //     }
        // });
    }

    public Task<GunImage?> FetchAsync(GunImage gunImage)
    {
        throw new NotImplementedException();
    }

    public async Task<GunImage> SaveAsync(GunId gunId, string extension, MemoryStream data)
    {
        ArgumentNullException.ThrowIfNull(gunId, nameof(gunId));
        ArgumentNullException.ThrowIfNullOrWhiteSpace(extension, nameof(extension));
        ArgumentNullException.ThrowIfNull(data, nameof(data));

        GunImageData gunImageData = new GunImageData()
        {
            Extension = extension
        };
        await _context.GunImages.AddAsync(gunImageData);
        await _context.SaveChangesAsync();

        GunImage gunImage = new GunImage(gunImageData.Id, extension);
        string path = gunImage.Path(gunId, _root);

        using (FileStream fs = new FileStream(path, FileMode.CreateNew))
        {
            await fs.WriteAsync(data.ToArray());
            await fs.FlushAsync();
        }

        return gunImage;
    }
}
