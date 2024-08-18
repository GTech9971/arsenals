using Arsenals.Domains;

namespace Arsenals.Infrastructure.FileStorage;

public class FSFileManager : IFileManager
{
    public void CreateDirectory(string path)
    {
        Directory.CreateDirectory(path);
    }

    public bool ExistsDirectory(string path)
    {
        return Directory.Exists(path);
    }
}
