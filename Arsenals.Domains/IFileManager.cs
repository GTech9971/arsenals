using System;

namespace Arsenals.Domains;

public interface IFileManager
{
    bool ExistsDirectory(string path);

    void CreateDirectory(string path);
}
