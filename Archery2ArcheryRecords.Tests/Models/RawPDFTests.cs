using Archery2ArcheryRecords.Core.Models;

namespace Archery2ArcheryRecords.Tests.Models;

public class RawPDFTest
{
    [Test]
    public async Task Constructor_ShouldInitializeFileName()
    {
        // Arrange
        var fileName = "test.pdf";
        using var fileStream = new MemoryStream();

        // Act
        var rawPDF = new RawPDF(fileName, fileStream);

        // Assert
        await Assert.That(rawPDF.FileName).IsEqualTo(fileName);
    }

    [Test]
    public void Dispose_ShouldDisposeStream()
    {
        // Arrange
        var fileStream = new MemoryStream();
        var rawPDF = new RawPDF("test.pdf", fileStream);

        // Act
        rawPDF.Dispose();

        // Assert
        Assert.Throws<ObjectDisposedException>(() => fileStream.ReadByte());
    }

    [Test]
    public async Task DisposeAsync_ShouldDisposeStreamAsync()
    {
        // Arrange
        var fileStream = new MemoryStream();
        var rawPDF = new RawPDF("test.pdf", fileStream);

        // Act
        await rawPDF.DisposeAsync();

        // Assert
        await Assert.ThrowsAsync<ObjectDisposedException>(async () => await fileStream.ReadAsync(new byte[1], 0, 1));
    }
}