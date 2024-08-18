using Arsenals.Domains.Guns;
using Microsoft.Extensions.Configuration;

namespace Arsenals.Infrastructure.FileStorage.Guns;

public class FSGunImageRepository : IGunImageRepository
{
    private static readonly string KEY = "images:root";
    private readonly IConfiguration _configuration;
    private readonly string _root;

    public FSGunImageRepository(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        _configuration = configuration;

        string? val = _configuration[KEY];
        ArgumentNullException.ThrowIfNullOrWhiteSpace(val, nameof(_configuration));
        _root = val;
    }

    public Task DeleteAsync(GunId id)
    {
        return Task.Run(() =>
        {
            if (File.Exists(id.ImagePath(_root)))
            {
                File.Delete(id.ImagePath(_root));
            }
        });
    }

    public Task<Uri?> FetchAsync(GunId id)
    {
        throw new NotImplementedException();
    }

    public async Task SaveAsync(GunId id, MemoryStream data)
    {
        using (FileStream fs = new FileStream(id.ImagePath(_root), FileMode.CreateNew))
        {
            await fs.WriteAsync(data.ToArray());
            await fs.FlushAsync();
        }
    }
}
