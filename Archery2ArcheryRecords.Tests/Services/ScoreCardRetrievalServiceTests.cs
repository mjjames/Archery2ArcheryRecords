using Archery2ArcheryRecords.Core.Errors;
using Archery2ArcheryRecords.Core.Models;
using Archery2ArcheryRecords.Core.Services;
using FluentResults;

namespace Archery2ArcheryRecords.Tests.Services;

public class ScoreCardRetrievalServiceTests
{
    private readonly ScoreCardRetrievalService _service = new(f => Task.FromResult<Stream>(File.OpenRead(f.FullPath)));

    [Test]
    public async Task LoadFiles_ShouldReturnValidResults_ForValidPdfFiles()
    {
        // Arrange
        var file = new ReadOnlyFile("Data/test.pdf", "application/pdf");
        var files = new List<FileBase> { file };

        // Act
        var results = _service.LoadFiles(files);
        var resultList = new List<Result<RawPDF>>();
        await foreach (var result in results)
        {
            resultList.Add(result);
        }

        // Assert
        await Assert.That(resultList.Count).IsEqualTo(1);
        await Assert.That(resultList[0].IsSuccess).IsTrue();
    }

    [Test]
    public async Task LoadFiles_ShouldReturnInvalidFileTypeError_ForNonPdfFiles()
    {
        // Arrange
        var file =new FileResult("test.txt","text/plain");
        var files = new List<FileBase> { file };

        // Act
        var results = _service.LoadFiles(files);
        var resultList = new List<Result<RawPDF>>();
        await foreach (var result in results)
        {
            resultList.Add(result);
        }

        // Assert
        await Assert.That(resultList.Count).IsEqualTo(1);
        await Assert.That(resultList[0].IsFailed).IsTrue();
        await Assert.That(resultList[0].Errors[0]).IsTypeOf<InvalidFileType>();
    }

    [Test]
    public async Task LoadFiles_ShouldReturnFileLoadError_OnException()
    {
        // Arrange
        var unnknownFile = new FileResult("unknown-file.pdf", "application/pdf");
        var invalidContentType =new FileResult("test.txt","text/plain");
        var validFile = new ReadOnlyFile("Data/test.pdf", "application/pdf");
        
        var files = new List<FileBase> { unnknownFile, invalidContentType, validFile };

        // Act
        var results = _service.LoadFiles(files);
        var resultList = new List<Result<RawPDF>>();
        await foreach (var result in results)
        {
            resultList.Add(result);
        }

        // Assert
        await Assert.That(resultList.Count).IsEqualTo(3);
        await Assert.That(resultList[0].IsFailed).IsTrue();
        await Assert.That(resultList[0].Errors[0]).IsTypeOf<FileLoadError>();
        await Assert.That(resultList[1].IsFailed).IsTrue();
        await Assert.That(resultList[1].Errors[0]).IsTypeOf<InvalidFileType>();
        await Assert.That(resultList[2].IsSuccess).IsTrue();
    }
}