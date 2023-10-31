using osu.Framework.IO.Stores;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using osu.Framework.Extensions;
using osu.Framework.Platform;

namespace Renako.Game.Stores;

public class RenakoStore : IResourceStore<byte[]>
{
    public readonly Storage Storage;

    public RenakoStore(Storage storage)
    {
        Storage = storage;
    }

    public byte[] Get(string name)
    {
        using (Stream stream = Storage.GetStream(name))
            return stream?.ReadAllBytesToArray();
    }

    public virtual async Task<byte[]> GetAsync(string name, CancellationToken cancellationToken = default)
    {
        using (Stream stream = Storage.GetStream(name))
            return await stream?.ReadAllBytesToArrayAsync(cancellationToken)!;
    }

    public Stream GetStream(string name)
    {
        return Storage.GetStream(name);
    }

    public IEnumerable<string> GetAvailableResources() =>
        Storage.GetDirectories(string.Empty).SelectMany(d => Storage.GetFiles(d)).ExcludeSystemFileNames();

    #region IDisposable Support

    public void Dispose()
    {
    }

    #endregion
}
