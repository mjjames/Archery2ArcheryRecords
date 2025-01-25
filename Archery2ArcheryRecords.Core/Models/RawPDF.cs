namespace Archery2ArcheryRecords.Core.Models;

public class RawPDF(string fileName, Stream fileStream) : IAsyncDisposable, IDisposable
{
    public string FileName { get; } = fileName;
    public Stream FileStream { get; } = fileStream;

    public void Dispose()
    {
        ((IDisposable)FileStream).Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return ((IAsyncDisposable)FileStream).DisposeAsync();
    }
}
